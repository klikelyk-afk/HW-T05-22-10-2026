using System.Diagnostics;

namespace App_511_SWAPI
{
    public partial class Form1 : Form
    {
        private NetworkHelper networkHelper = new();
        private PersonDetailsPresenterHw_05 detailsPresenter;


        public Form1()
        {
            InitializeComponent();

            progressBar1.Visible = false;

            this.Load += Form1_Load;

            detailsPresenter = new PersonDetailsPresenterHw_05(label1, label2, label3, label4);

            var testPattern = new TestPattern();
        }

        private async void Form1_Load(object? sender, EventArgs e)
        {
            AnimateLoading(true);

            await LoadData();

            AnimateLoading(false);
        }

        private async Task LoadData()
        {
            var swapiManager = new SwapiInfoManager(networkHelper);
            var imagesManager = new ImageDataManager(networkHelper);

            InfoBase infoBase = await swapiManager.GetBaseInfo();
            List<PersonData> personData = await swapiManager.GetPeoplesInfo(infoBase.PeoplesUrl);

            List<ImageItem> imageItems = await imagesManager.GetImagesDataInfo();

            personData = DataMerger.MergeData(personData, imageItems);

            FillPersonsList(personData);
        }

        private async void btnView_Click(object sender, EventArgs e)
        {
            RequestResponse response = await networkHelper.SendRequest("https://swapi.info/api/");
            if (response.Status == RequestStatus.Ok)
            {
                Debug.WriteLine(response.Data);
            }
        }

        private void FillPersonsList(List<PersonData> persons)
        {
            listBoxPersons.Items.Clear();
            foreach (var item in persons)
            {
                listBoxPersons.Items.Add(item);
            }
        }

        private async Task ShowImage(string imageUrl)
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
                client.DefaultRequestHeaders.Add("Referer", "https://starwars.fandom.com/");
                byte[] bytes = await client.GetByteArrayAsync(imageUrl);
                using var vipsImage = NetVips.Image.NewFromBuffer(bytes);
                byte[] pngBuffer = vipsImage.WriteToBuffer(".png");
                using var ms = new MemoryStream(pngBuffer);
                pictureBox1.Image?.Dispose();
                pictureBox1.Image = System.Drawing.Image.FromStream(ms);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error decoding: " + ex.Message);
            }
        }

        private async void listBoxPersons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxPersons.SelectedItem is PersonData person)
            {

                detailsPresenter.Display(person);

                pictureBox1.Image?.Dispose();
                // Load base "error" or "invisible" (transparent) image.
                AnimateLoading(true);

                await ShowImage(person.ImageUrl);

                AnimateLoading(false);
            }
        }

        private void AnimateLoading(bool isLoading)
        {
            progressBar1.Visible = isLoading;
        }
    }
}
