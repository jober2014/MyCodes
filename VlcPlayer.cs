using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Collections.Generic;
using System.Text;

namespace vlcPlayer
{
    /// <summary>
    /// 万成播放器
    /// </summary>
    class VlcPlayer
    {
        private IntPtr libvlc_instance_;
        private IntPtr libvlc_media_player_;

        private double duration_;

        /// <summary>
        /// 播放方法
        /// </summary>
        /// <param name="pluginPath">路径</param>
        public VlcPlayer(string pluginPath)
        {
            string plugin_arg = "--plugin-path=" + pluginPath;
            string[] arguments = { "-I", "dummy", "--ignore-config", "--no-video-title", plugin_arg };
            libvlc_instance_ = VLCApi.libvlc_new(arguments);

            libvlc_media_player_ = VLCApi.libvlc_media_player_new(libvlc_instance_);
        }
        /// <summary>
        /// 读取窗体句柄
        /// </summary>
        /// <param name="wndHandle"></param>
        public void SetRenderWindow(int wndHandle)
        {
            if (libvlc_instance_ != IntPtr.Zero && wndHandle != 0)
            {
                VLCApi.libvlc_media_player_set_hwnd(libvlc_media_player_, wndHandle);
            }
        }
        /// <summary>
        ///播放文件
        /// </summary>
        /// <param name="filePath">路径</param>
        public void PlayFile(string filePath)
        {
            IntPtr libvlc_media = VLCApi.libvlc_media_new_path(libvlc_instance_, filePath);
            if (libvlc_media != IntPtr.Zero)
            {
                VLCApi.libvlc_media_parse(libvlc_media);
                duration_ = VLCApi.libvlc_media_get_duration(libvlc_media) / 1000.0;

                VLCApi.libvlc_media_player_set_media(libvlc_media_player_, libvlc_media);
                VLCApi.libvlc_media_release(libvlc_media);

                VLCApi.libvlc_media_player_play(libvlc_media_player_);
              
            }
        }
        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            if (libvlc_media_player_ != IntPtr.Zero)
            {
                VLCApi.libvlc_media_player_pause(libvlc_media_player_);
            }
        }
        public void Play() {
            if (libvlc_media_player_ != IntPtr.Zero)
            {
                VLCApi.libvlc_media_player_play(libvlc_media_player_);
            }
        }
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            if (libvlc_media_player_ != IntPtr.Zero)
            {
                VLCApi.libvlc_media_player_stop(libvlc_media_player_);
            }
        }
        /// <summary>
        /// 获取播放时间
        /// </summary>
        /// <returns></returns>
        public double GetPlayTime()
        {
            return VLCApi.libvlc_media_player_get_time(libvlc_media_player_) / 1000.0;
        }
        /// <summary>
        /// 设置播放时间
        /// </summary>
        /// <param name="seekTime"></param>
        public void SetPlayTime(double seekTime)
        {
            VLCApi.libvlc_media_player_set_time(libvlc_media_player_, (Int64)(seekTime * 1000));
        }
        /// <summary>
        /// 获取声音
        /// </summary>
        /// <returns></returns>
        public int GetVolume()
        {
            return VLCApi.libvlc_audio_get_volume(libvlc_media_player_);
        }
        /// <summary>
        /// 设置声音
        /// </summary>
        /// <param name="volume"></param>
        public void SetVolume(int volume)
        {
            VLCApi.libvlc_audio_set_volume(libvlc_media_player_, volume);
        }
        /// <summary>
        /// 全屏
        /// </summary>
        /// <param name="istrue">是否为真</param>
        public void SetFullScreen(bool istrue)
        {

            if (istrue)
            {
                VLCApi.libvlc_set_fullscreen(libvlc_media_player_, 1);
            }
            else
            {
                VLCApi.libvlc_set_fullscreen(libvlc_media_player_, 0);
            }

        }

        public double Duration()
        {
            return duration_;
        }

        public string Version()
        {
            return VLCApi.libvlc_get_version();
        }
    }
 
  
}
