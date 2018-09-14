using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using FFmpegInterop;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private FFmpegInteropMSS FFmpegMSS;

        private async void LoadVideoFile(object sender, TappedRoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.VideosLibrary;
            picker.FileTypeFilter.Add(".mp4");
            StorageFile file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                if (mediaPlayer.CanPause == true)
                {
                    try
                    {
                        mediaPlayer.Pause();
                    }
                    catch (Exception exception)
                    {
//                    System.Console.WriteLine(exception);
                        throw;
                    }
                }
                else
                {
                    try
                    {
                        mediaPlayer.Stop();
                    }
                    catch (Exception exception)
                    {
//                        System.Console.WriteLine(exception);
                        throw;
                    }
                }

                IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);

                try
                {
                    FFmpegMSS = FFmpegInteropMSS.CreateFFmpegInteropMSSFromStream(stream, true, true);
                    MediaStreamSource mss = FFmpegMSS.GetMediaStreamSource();

                    if (mss != null)
                    {
                        mediaPlayer.AreTransportControlsEnabled = true;

                        mediaPlayer.TransportControls.IsStopButtonVisible = true;
                        mediaPlayer.TransportControls.IsStopEnabled = true;

                        mediaPlayer.TransportControls.IsFastForwardButtonVisible = true;
                        mediaPlayer.TransportControls.IsFastForwardEnabled = true;

                        mediaPlayer.TransportControls.IsFastRewindButtonVisible = true;
                        mediaPlayer.TransportControls.IsFastRewindEnabled = true;
                        mediaPlayer.SetMediaStreamSource(mss);

                        mediaPlayer.Play();
                    }
                    else
                    {
                        var msg = new MessageDialog("errs");
                        await msg.ShowAsync();
                    }
                }
                catch (Exception exception)
                {
                    //                System.Console.WriteLine(exception);
                    //                throw;
                }
            }
        }

        private void LoadSongFile(object sender, TappedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}