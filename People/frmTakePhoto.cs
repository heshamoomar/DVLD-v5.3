using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using AForge.Video;
using AForge.Video.DirectShow;

namespace DVLD_v1._0.People
{
    public partial class frmTakePhoto : Form
    {
        public delegate void DataBackEventHandler(object sender, string imagePath);
        public event DataBackEventHandler DataBack;

        private FilterInfoCollection videoDevices; // List of camera devices
        private VideoCaptureDevice videoSource;    // The video source (camera)
        private Bitmap currentFrame;               // The current frame from the camera

        public frmTakePhoto()
        {
            InitializeComponent();
        }
        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // Get the current frame and display it in the PictureBox
            if (currentFrame != null)
                currentFrame.Dispose(); // Dispose of previous frame to avoid memory leaks

            currentFrame = (Bitmap)eventArgs.Frame.Clone();
            pictureBoxCamera.Image = currentFrame;
        }

        private void frmTakePhoto_Load(object sender, EventArgs e)
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (videoDevices.Count == 0)
            {
                MessageBox.Show("No camera found.");
                return;
            }

            // Use the first available camera
            videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);

            // Set the NewFrame event handler
            videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);

            // Start the video source
            videoSource.Start();
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
            }
            btnClear.Enabled = true;
            btnUsePhoto.Enabled = true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            pictureBoxCamera.Image.Dispose();
            pictureBoxCamera.Image = null;
            videoSource.Start();

            btnClear.Enabled = false;
            btnUsePhoto.Enabled = false;
        }

        private void frmTakePhoto_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
            }

            currentFrame?.Dispose();
        }

        private void btnUsePhoto_Click(object sender, EventArgs e)
        {
            // Define the folder path
            string folderPath = @"C:\captured_photos";

            // Create the folder if it doesn't exist
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Generate a GUID for the image filename
            string imageName = Guid.NewGuid().ToString() + ".jpg";

            // Combine the folder path and image name to create the full path
            string imagePath = Path.Combine(folderPath, imageName);

            // Save the current frame as a JPEG file
            currentFrame.Save(imagePath, System.Drawing.Imaging.ImageFormat.Jpeg);

            //MessageBox.Show($"Image saved at: {imagePath}");
            DataBack?.Invoke(this, imagePath);
            this.Close();
        }

    }
}
