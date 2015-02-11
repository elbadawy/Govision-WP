﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ZXing.Mobile;
using ZXing;
using System.ComponentModel;

namespace Govision
{
    public partial class start : PhoneApplicationPage
    {

        public static MobileBarcodeScanningOptions ScanningOptions = new MobileBarcodeScanningOptions();
        public static MobileBarcodeScannerBase Scanner;
        public static Result LastScanResult;
        public static Action<Result> FinishedAction;

        /*public static event Action<bool> OnRequestTorch;
        public static event Action OnRequestToggleTorch;
        public static event Action OnRequestAutoFocus;
        public static event Action OnRequestCancel;
        public static event Func<bool> OnRequestIsTorchOn;
        public static event Func<bool> OnRequestCanTorch;*/

        public start()
        {
            InitializeComponent();

            PhoneApplicationService.Current.Activated += (s, e) =>
            {
                scanner();
            };
        }

        #region Requests 
        /*
        public static bool RequestIsTorchOn()
        {
            var evt = OnRequestIsTorchOn;
            return evt != null && evt();
        }

        public static bool RequestCanTorch()
        {
            var evt = OnRequestCanTorch;
            return evt != null && evt();
        }

        public static void RequestTorch(bool on)
        {
            var evt = OnRequestTorch;
            if (evt != null)
                evt(on);
        }

        public static void RequestToggleTorch()
        {
            var evt = OnRequestToggleTorch;
            if (evt != null)
                evt();
        }

        public static void RequestAutoFocus()
        {
            var evt = OnRequestAutoFocus;
            if (evt != null)
                evt();
        }

        public static void RequestCancel()
        {
            var evt = OnRequestCancel;
            if (evt != null)
                evt();
        }

        void RequestAutoFocusHandler()
        {
            if (scannerControl != null)
                scannerControl.AutoFocus();
        }

        void RequestTorchHandler(bool on)
        {
            if (scannerControl != null)
                scannerControl.Torch(on);
        }

        void RequestToggleTorchHandler()
        {
            if (scannerControl != null)
                scannerControl.ToggleTorch();
        }

        void RequestCancelHandler()
        {
            if (scannerControl != null)
                scannerControl.Cancel();
        }

        bool RequestIsTorchOnHandler()
        {
            if (scannerControl != null)
                return scannerControl.IsTorchOn;

            return false;
        }

        bool RequestCanTorchHandler()
        {
            if (scannerControl != null)
                return scannerControl.CanTorch;

            return false;
        }
        */
        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            scanner();
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            try
            {
                /*OnRequestAutoFocus -= RequestAutoFocusHandler;
                OnRequestTorch -= RequestTorchHandler;
                OnRequestToggleTorch -= RequestToggleTorchHandler;
                OnRequestCancel -= RequestCancelHandler;
                OnRequestIsTorchOn -= RequestIsTorchOnHandler;*/

                scannerControl.StopScanning();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            base.OnNavigatingFrom(e);
        }

        private void scanner()
        { 
            try
            {
                //set scanner options (Set PossibleFormats to QR_CODE)
                ScanningOptions.PossibleFormats = new List<ZXing.BarcodeFormat>() { 
                    ZXing.BarcodeFormat.QR_CODE
                    //ZXing.BarcodeFormat.AZTEC,
                    //ZXing.BarcodeFormat.DATA_MATRIX,
                };

                ScanningOptions.AutoRotate = true;
                //ScanningOptions.TryHarder = true;
                scannerControl.ScanningOptions = ScanningOptions;

                //OnRequest events handlers
                /*OnRequestAutoFocus += RequestAutoFocusHandler;
                OnRequestTorch += RequestTorchHandler;
                OnRequestToggleTorch += RequestToggleTorchHandler;
                OnRequestCancel += RequestCancelHandler;
                OnRequestIsTorchOn += RequestIsTorchOnHandler;
                OnRequestCanTorch += RequestCanTorchHandler;*/

                //start the scanner
                scannerControl.StartScanning(HandleResult, ScanningOptions);

                //if device doesn't have a flash, disable flash button
                if (scannerControl.CanTorch == false)
                {
                    ApplicationBar.Buttons.RemoveAt(0);

                    ApplicationBarIconButton flashButton = new ApplicationBarIconButton();
                    flashButton.Text = "Flash";
                    flashButton.IconUri = new Uri("/Assets/icons/appbar.camera.flash.png", UriKind.Relative);
                    flashButton.Click += _flashButton;
                    flashButton.IsEnabled = false;
                    ApplicationBar.Buttons.Add(flashButton);
                }
            }
            catch (Exception ex) { MessageBox.Show("Error" +"\n" + ex.Message + "\n" + ex.HResult); }
        }

        void HandleResult(ZXing.Result result)
        {
            //NavigationService.Navigate(new Uri("/DataBeacon.xaml?t=" + result.Text, UriKind.Relative));
            MessageBox.Show(result.Text);
        }

        private void _flashButton(object sender, EventArgs e)
        {
            scannerControl.ToggleTorch();
            scannerControl.AutoFocus();
            scannerControl.ToggleTorch();
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            Application.Current.Terminate();
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/History.xaml", UriKind.Relative));
        }
    }
}