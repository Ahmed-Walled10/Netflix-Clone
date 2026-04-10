using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

class Program
{
    static void Main()
    {
        string assemblyPath = @"c:\Users\micro\Desktop\study\Netflix-Clone\Backend-Netflix-CLone\Netflix-Clone\NetflixClone.Application\bin\Debug\net9.0\NetflixClone.Application.dll";
        string sourceDir = @"c:\Users\micro\Desktop\study\Netflix-Clone\Backend-Netflix-CLone\Netflix-Clone\NetflixClone.Application\Features";
        
        var assembly = Assembly.LoadFrom(assemblyPath);
        var requestTypes = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i.Name == "IRequest" || (i.IsGenericType && i.GetGenericTypeDefinition().Name == "IRequest`1")))
            .Where(t => t.IsClass || t.IsValueType)
            .ToList();

        var allFiles = Directory.GetFiles(sourceDir, "*.cs", SearchOption.AllDirectories);

        Console.WriteLine("| Feature | Class name | Validator created ✓ |");
        Console.WriteLine("|---|---|---|");

        foreach (var type in requestTypes)
        {
            if (type.Namespace != null && type.Namespace.Contains("Authentication")) continue; // Skipped

            var sourceFile = allFiles.FirstOrDefault(f => Path.GetFileNameWithoutExtension(f) == type.Name);
            if (sourceFile == null) continue;

            string folder = Path.GetDirectoryName(sourceFile);
            string validatorName = type.Name + "Validator";
            string validatorPath = Path.Combine(folder, validatorName + ".cs");

            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using FluentValidation;");
            sb.AppendLine();
            sb.AppendLine($"namespace {type.Namespace}");
            sb.AppendLine("{");
            sb.AppendLine($"    public class {validatorName} : AbstractValidator<{type.Name}>");
            sb.AppendLine("    {");
            sb.AppendLine($"        public {validatorName}()");
            sb.AppendLine("        {");

            if (props.Length == 0)
            {
                sb.AppendLine("            // No properties to validate in this request.");
            }
            else
            {
                foreach (var prop in props)
                {
                    string pName = prop.Name;
                    Type pType = prop.PropertyType;
                    bool isNullable = Nullable.GetUnderlyingType(pType) != null || (!pType.IsValueType && pType != typeof(string));
                    string realType = Nullable.GetUnderlyingType(pType)?.Name ?? pType.Name;

                    string rule = $"            RuleFor(x => x.{pName})";
                    bool hasRules = false;

                    if (realType == "String")
                    {
                        rule += $"\n                .NotEmpty().WithMessage(\"{pName} is required.\")";
                        hasRules = true;
                        
                        if (pName.IndexOf("Email", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            rule += $"\n                .EmailAddress().WithMessage(\"{pName} must be a valid email.\")";
                        }
                        else if (pName.IndexOf("Password", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            rule += $"\n                .MinimumLength(8).WithMessage(\"{pName} must be at least 8 characters.\")" +
                                    $"\n                .Matches(@\"[A-Z]\").WithMessage(\"{pName} must contain uppercase.\")" +
                                    $"\n                .Matches(@\"[0-9]\").WithMessage(\"{pName} must contain a number.\")";
                        }
                        else if (pName.IndexOf("Description", StringComparison.OrdinalIgnoreCase) >= 0)
                            rule += $"\n                .MaximumLength(500).WithMessage(\"{pName} must not exceed 500 characters.\")";
                        else if (pName.IndexOf("Name", StringComparison.OrdinalIgnoreCase) >= 0 || pName.IndexOf("Title", StringComparison.OrdinalIgnoreCase) >= 0)
                            rule += $"\n                .MaximumLength(100).WithMessage(\"{pName} must not exceed 100 characters.\")";
                        else 
                            rule += $"\n                .MaximumLength(200).WithMessage(\"{pName} must not exceed 200 characters.\")"; // fallback
                    }
                    else if (realType == "Int32" || realType == "Decimal" || realType == "Double")
                    {
                        if (pName.IndexOf("Id", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            pName.IndexOf("Price", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            pName.IndexOf("Count", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            rule += $"\n                .GreaterThan(0).WithMessage(\"{pName} must be greater than zero.\")";
                            hasRules = true;
                        }
                    }
                    else if (realType == "Guid")
                    {
                        rule += $"\n                .NotEqual(Guid.Empty).WithMessage(\"{pName} must be a valid identifier.\")";
                        hasRules = true;
                    }
                    else if (realType == "DateTime")
                    {
                        if (pName.IndexOf("End", StringComparison.OrdinalIgnoreCase) >= 0 || pName.IndexOf("Future", StringComparison.OrdinalIgnoreCase) >= 0 || pName.IndexOf("Expire", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            rule += $"\n                .GreaterThan(DateTime.UtcNow).WithMessage(\"{pName} must be in the future.\")";
                            hasRules = true;
                        }
                    }

                    if (hasRules)
                    {
                        if (isNullable) 
                        {
                            sb.AppendLine($"            When(x => x.{pName} != null, () => {{");
                            sb.AppendLine(rule + ";");
                            sb.AppendLine("            });");
                        }
                        else 
                        {
                            sb.AppendLine(rule + ";\n");
                        }
                    }
                }
            }

            if (props.Any(p => p.Name.Equals("Password")) && props.Any(p => p.Name.Equals("ConfirmPassword")))
            {
                sb.AppendLine("            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage(\"ConfirmPassword must match Password.\");");
            }

            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            File.WriteAllText(validatorPath, sb.ToString());

            string feature = type.Namespace.Split('.').FirstOrDefault(n => n != "NetflixClone" && n != "Application" && n != "Features");
            string relativeNs = type.Namespace.Replace("NetflixClone.Application.Features.", "");
            var parts = relativeNs.Split('.');
            string dispFeature = parts.Length > 0 ? parts[0] : relativeNs;
            
            Console.WriteLine($"| {dispFeature} | {type.Name} | ✓ |");
        }
    }
}
