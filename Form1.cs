namespace ImageViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        PictureBox[] pB;
        string[] fileNames;
        int startIndex = 0;
        int endIndex = 0;
        private void Form1_Load(object sender, EventArgs e)
        {
            pB = new PictureBox[] { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5,pictureBox6 };
            txtRoute.Enabled = false;
            foreach (var pb in pB)
            {
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.BorderStyle = BorderStyle.FixedSingle;
            }
            pbBig.SizeMode = PictureBoxSizeMode.StretchImage;
            pbBig.BorderStyle = BorderStyle.FixedSingle;
            openFileDialog1.Multiselect = true;
            btnLeft.Enabled = false;
            btnRight.Enabled = false;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
         
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
       
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
         
        }

        private void UpdatePictureBoxes()
        {

        }
    }
}