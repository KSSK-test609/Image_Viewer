namespace ImageViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        PictureBox[] pB = Array.Empty<PictureBox>();
        string[] fileNames = Array.Empty<string>();
        int startIndex = 0;
        int endIndex = 0;

        private void Form1_Load(object? sender, EventArgs e)
        {
            pB = new PictureBox[]
            {
                pictureBox1, pictureBox2, pictureBox3,
                pictureBox4, pictureBox5, pictureBox6
            };

            txtRoute.Enabled = false;
            foreach (var pb in pB)
            {
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.BorderStyle = BorderStyle.FixedSingle;
            }

            pbBig.SizeMode = PictureBoxSizeMode.StretchImage;
            pbBig.BorderStyle = BorderStyle.FixedSingle;
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter =
                "圖片檔案 (*.jpg;*.jpeg;*.png;*.bmp;*.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            openFileDialog1.FileName = string.Empty;
            btnLeft.Enabled = false;
            btnRight.Enabled = false;
        }

        private void btnOpen_Click(object? sender, EventArgs e)
        {
            // 取消選檔時，保留目前顯示的內容。
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            fileNames = openFileDialog1.FileNames;
            startIndex = 0;

            pbBig.Image?.Dispose();
            pbBig.Image = null;
            txtRoute.Clear();
            UpdatePictureBoxes();
        }

        private void pictureBox1_Click(object? sender, EventArgs e)
        {
            // 六個小圖共用這個事件，sender 是實際被點到的小圖。
            if (sender is not PictureBox smallPicture || smallPicture.Image == null)
                return;

            int position = Array.IndexOf(pB, smallPicture);
            if (position < 0)
                return;

            int index = startIndex + position;

            // 大圖持有獨立副本，切頁釋放小圖時不會影響大圖。
            Image enlargedImage = (Image)smallPicture.Image.Clone();
            pbBig.Image?.Dispose();
            pbBig.Image = enlargedImage;
            txtRoute.Text = fileNames[index];
        }

        private void btnLeft_Click(object? sender, EventArgs e)
        {
            if (startIndex == 0)
                return;

            startIndex -= pB.Length;
            UpdatePictureBoxes();
        }

        private void btnRight_Click(object? sender, EventArgs e)
        {
            if (startIndex + pB.Length >= fileNames.Length)
                return;

            startIndex += pB.Length;
            UpdatePictureBoxes();
        }

        private void UpdatePictureBoxes()
        {
            // 結束位置不包含 endIndex，本頁最多顯示六張。
            endIndex = Math.Min(startIndex + pB.Length, fileNames.Length);
            List<string> failedFiles = new List<string>();

            for (int i = 0; i < pB.Length; i++)
            {
                pB[i].Image?.Dispose();
                pB[i].Image = null;

                int index = startIndex + i;
                if (index >= endIndex)
                    continue;

                try
                {
                    // 讀入後建立副本，避免圖片檔案持續被鎖住。
                    using (Image original = Image.FromFile(fileNames[index]))
                    {
                        pB[i].Image = new Bitmap(original);
                    }
                }
                catch (Exception ex) when (ex is IOException ||
                    ex is UnauthorizedAccessException ||
                    ex is ArgumentException || ex is OutOfMemoryException)
                {
                    failedFiles.Add(fileNames[index]);
                }
            }

            btnLeft.Enabled = startIndex > 0;
            btnRight.Enabled = endIndex < fileNames.Length;

            if (failedFiles.Count > 0)
            {
                MessageBox.Show(
                    "下列圖片無法讀取，請確認檔案存在且格式正確：\n" +
                    string.Join("\n", failedFiles),
                    "圖片讀取失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // 關閉程式時，釋放自行載入的圖片資源。
            foreach (var pb in pB)
            {
                pb.Image?.Dispose();
                pb.Image = null;
            }

            pbBig.Image?.Dispose();
            pbBig.Image = null;
            base.OnFormClosed(e);
        }
    }
}
