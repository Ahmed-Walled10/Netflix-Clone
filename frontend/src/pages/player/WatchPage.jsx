import { useState, useEffect, useRef } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { catalogService } from '../../api/catalogService';
import Hls from 'hls.js';
import { FiArrowLeft, FiMaximize, FiMinimize, FiVolume2, FiVolumeX } from 'react-icons/fi';

const WatchPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const videoRef = useRef(null);
  const containerRef = useRef(null);
  const [streamData, setStreamData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [isPlaying, setIsPlaying] = useState(false);
  const [currentTime, setCurrentTime] = useState(0);
  const [duration, setDuration] = useState(0);
  const [volume, setVolume] = useState(1);
  const [isMuted, setIsMuted] = useState(false);
  const [isFullscreen, setIsFullscreen] = useState(false);
  const [showControls, setShowControls] = useState(true);
  const hideControlsTimer = useRef(null);

  useEffect(() => {
    loadStream();
    return () => {
      if (hideControlsTimer.current) clearTimeout(hideControlsTimer.current);
    };
  }, [id]);

  const loadStream = async () => {
    try {
      const { data } = await catalogService.playContent(id);
      setStreamData(data);
      setupPlayer(data);
    } catch (err) {
      setError(err.response?.data?.message || err.response?.data || 'Failed to load stream. Check your subscription.');
    } finally {
      setLoading(false);
    }
  };

  const setupPlayer = (data) => {
    const video = videoRef.current;
    if (!video) return;

    const url = data.manifestUrl || data.streamingUrl;

    if (url.includes('.m3u8') && Hls.isSupported()) {
      const hls = new Hls({ startLevel: -1 });
      hls.loadSource(url);
      hls.attachMedia(video);
      hls.on(Hls.Events.MANIFEST_PARSED, () => {
        video.play().catch(() => {});
      });
    } else if (video.canPlayType('application/vnd.apple.mpegurl')) {
      video.src = url;
      video.play().catch(() => {});
    } else {
      video.src = url;
      video.play().catch(() => {});
    }
  };

  const handleTimeUpdate = () => {
    if (videoRef.current) {
      setCurrentTime(videoRef.current.currentTime);
      setDuration(videoRef.current.duration || 0);
    }
  };

  const handleSeek = (e) => {
    const rect = e.currentTarget.getBoundingClientRect();
    const pos = (e.clientX - rect.left) / rect.width;
    if (videoRef.current) {
      videoRef.current.currentTime = pos * videoRef.current.duration;
    }
  };

  const togglePlay = () => {
    if (!videoRef.current) return;
    if (videoRef.current.paused) {
      videoRef.current.play();
      setIsPlaying(true);
    } else {
      videoRef.current.pause();
      setIsPlaying(false);
    }
  };

  const toggleMute = () => {
    if (!videoRef.current) return;
    videoRef.current.muted = !videoRef.current.muted;
    setIsMuted(!isMuted);
  };

  const toggleFullscreen = () => {
    if (!containerRef.current) return;
    if (!document.fullscreenElement) {
      containerRef.current.requestFullscreen();
      setIsFullscreen(true);
    } else {
      document.exitFullscreen();
      setIsFullscreen(false);
    }
  };

  const handleMouseMove = () => {
    setShowControls(true);
    if (hideControlsTimer.current) clearTimeout(hideControlsTimer.current);
    hideControlsTimer.current = setTimeout(() => {
      if (isPlaying) setShowControls(false);
    }, 3000);
  };

  const formatTime = (seconds) => {
    if (!seconds || isNaN(seconds)) return '0:00';
    const m = Math.floor(seconds / 60);
    const s = Math.floor(seconds % 60);
    return `${m}:${s.toString().padStart(2, '0')}`;
  };

  if (loading) return <div className="loading-screen"><div className="spinner" /></div>;

  if (error) {
    return (
      <div style={{ minHeight: '100vh', display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', background: '#000', color: 'var(--text-primary)', padding: 'var(--space-xl)', textAlign: 'center' }}>
        <p style={{ fontSize: 'var(--fs-xl)', marginBottom: 'var(--space-xl)' }}>{error}</p>
        <button className="btn btn--primary" onClick={() => navigate(-1)} style={{ width: 'auto' }}>Go Back</button>
      </div>
    );
  }

  return (
    <div
      ref={containerRef}
      onMouseMove={handleMouseMove}
      style={{ position: 'fixed', inset: 0, background: '#000', zIndex: 'var(--z-player)', cursor: showControls ? 'default' : 'none' }}
    >
      <video
        ref={videoRef}
        onTimeUpdate={handleTimeUpdate}
        onPlay={() => setIsPlaying(true)}
        onPause={() => setIsPlaying(false)}
        onClick={togglePlay}
        style={{ width: '100%', height: '100%', objectFit: 'contain' }}
      />

      {/* Controls Overlay */}
      <div style={{
        position: 'absolute',
        inset: 0,
        display: 'flex',
        flexDirection: 'column',
        justifyContent: 'space-between',
        opacity: showControls ? 1 : 0,
        transition: 'opacity 0.3s',
        pointerEvents: showControls ? 'auto' : 'none',
      }}>
        {/* Top Bar */}
        <div style={{ display: 'flex', alignItems: 'center', padding: 'var(--space-xl)', gap: 'var(--space-md)', background: 'linear-gradient(180deg, rgba(0,0,0,0.7), transparent)' }}>
          <button onClick={() => navigate(-1)} style={{ background: 'none', border: 'none', color: 'white', cursor: 'pointer', display: 'flex', alignItems: 'center', gap: 'var(--space-sm)', fontSize: 'var(--fs-lg)' }}>
            <FiArrowLeft size={24} /> Back
          </button>
          {streamData?.quality && (
            <span style={{ marginLeft: 'auto', background: 'var(--netflix-red)', padding: '4px 10px', borderRadius: 'var(--radius-sm)', fontSize: 'var(--fs-xs)', fontWeight: 700 }}>
              {streamData.quality}
            </span>
          )}
        </div>

        {/* Center Play Button */}
        <div style={{ display: 'flex', justifyContent: 'center' }}>
          <button onClick={togglePlay} style={{ background: 'rgba(0,0,0,0.5)', border: '2px solid white', borderRadius: '50%', width: 70, height: 70, display: 'flex', alignItems: 'center', justifyContent: 'center', color: 'white', fontSize: '2rem', cursor: 'pointer' }}>
            {isPlaying ? '⏸' : '▶'}
          </button>
        </div>

        {/* Bottom Controls */}
        <div style={{ padding: 'var(--space-md) var(--space-xl)', background: 'linear-gradient(transparent, rgba(0,0,0,0.8))' }}>
          {/* Progress Bar */}
          <div onClick={handleSeek} style={{ height: 4, background: 'rgba(255,255,255,0.2)', borderRadius: 2, cursor: 'pointer', marginBottom: 'var(--space-md)', position: 'relative' }}>
            <div style={{ height: '100%', background: 'var(--netflix-red)', borderRadius: 2, width: `${duration > 0 ? (currentTime / duration) * 100 : 0}%`, transition: 'width 0.1s' }} />
            <div style={{ position: 'absolute', top: -6, width: 14, height: 14, borderRadius: '50%', background: 'var(--netflix-red)', left: `${duration > 0 ? (currentTime / duration) * 100 : 0}%`, transform: 'translateX(-50%)' }} />
          </div>

          <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}>
            <div style={{ display: 'flex', alignItems: 'center', gap: 'var(--space-md)' }}>
              <button onClick={togglePlay} style={{ background: 'none', border: 'none', color: 'white', cursor: 'pointer', fontSize: 'var(--fs-xl)' }}>
                {isPlaying ? '⏸' : '▶'}
              </button>
              <button onClick={toggleMute} style={{ background: 'none', border: 'none', color: 'white', cursor: 'pointer' }}>
                {isMuted ? <FiVolumeX size={20} /> : <FiVolume2 size={20} />}
              </button>
              <span style={{ fontSize: 'var(--fs-sm)', color: 'var(--text-secondary)' }}>
                {formatTime(currentTime)} / {formatTime(duration)}
              </span>
            </div>
            <button onClick={toggleFullscreen} style={{ background: 'none', border: 'none', color: 'white', cursor: 'pointer' }}>
              {isFullscreen ? <FiMinimize size={20} /> : <FiMaximize size={20} />}
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default WatchPage;
