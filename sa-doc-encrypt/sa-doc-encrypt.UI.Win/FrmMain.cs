using Microsoft.Extensions.Logging;
using sa_doc_encrypt.Domain.Contracts;
using sa_doc_encrypt.Domain.Dto;

namespace sa_doc_encrypt.UI.Win
{
    public partial class FrmMain : Form
    {
        private readonly ILogger<FrmMain> _logger;
        private readonly ICryptoApiClient _cryptoApiClient;
        private readonly ICryptoOpers _cryptoOpers;

        public FrmMain(ILogger<FrmMain> logger, ICryptoApiClient cryptoApiClient, ICryptoOpers cryptoOpers)
        {
            _logger = logger;
            _cryptoApiClient = cryptoApiClient;
            _cryptoOpers = cryptoOpers;

            InitializeComponent();

            PopulateCombo();
        }

        private void PopulateCombo()
        {
            try
            {
                List<CryptoOperation> operations = new();

                foreach (CryptoOperation operation in Enum.GetValues(typeof(CryptoOperation)))
                {
                    operations.Add(operation);
                }

                cmbOperation.DataSource = operations;
                cmbOperation.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetFile");
                txtOut.Text = $"Error poblando combo de operaciones:\r\n{ex.Message}";
            }
        }

        private void BtnFindFile_Click(object sender, EventArgs e)
        {
            GetFile();
        }

        private void GetFile()
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtSrcFile.Text = ofd.FileName;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetFile");
                txtOut.Text = $"Error buscando archivo origen:\r\n{ex.Message}";
            }
        }

        private async void BtnProcess_Click(object sender, EventArgs e)
        {
            await ProcessFile();
        }

        private async Task ProcessFile()
        {
            SetUIState(enable: false);

            try
            {
                FileInfo file = new FileInfo(txtSrcFile.Text);
                bool getCryptoOperationOk = Enum.TryParse(cmbOperation.Text, out CryptoOperation operation);

                if (!getCryptoOperationOk)
                {
                    txtOut.Text = $"Error recuperando operación criptográfica";
                    return;
                }

                CryptoOperationResult result = await PerformProcess(file, operation);

                if (!result.Success)
                {
                    txtOut.Text = $"Error en api procesando archivo:\r\n{result.Message}";
                    return;
                }

                FileInfo? fileResult = await _cryptoApiClient.GetFile(result.Url);
                if (fileResult == null)
                {
                    txtOut.Text = $"Error descargando archivo, por favor verificar log de programa";
                    return;
                }

                txtOut.Text = fileResult.FullName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetFile");
                txtOut.Text = $"Error general procesando archivo:\r\n{ex.Message}";
            }
            finally
            {
                SetUIState(enable: true);
            }
        }

        private async Task<CryptoOperationResult> PerformProcess(FileInfo file, CryptoOperation operation)
        {
            if (operation == CryptoOperation.Cifrar)
            {
                return await _cryptoOpers.EncryptFile(file);
            }
            else
            {
                return await _cryptoOpers.DecryptFile(file);
            }
        }

        private void SetUIState(bool enable)
        {
            this.Cursor = enable ? Cursors.Default : Cursors.WaitCursor;
            txtOut.Enabled = enable;
            txtSrcFile.Enabled = enable;
            btnFindFile.Enabled = enable;
            btnProcess.Enabled = enable;
            cmbOperation.Enabled = enable;
        }
    }
}
