using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TempleteCreator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // maximize butonunu gizle
            this.MaximizeBox = false;
            progressBar1.Visible = false;


        }

        private async void button1_Click(object sender, EventArgs e)
        {

            string projectName = textName.Text.Trim().Replace(" ", "");
            string location = fileLocationBrowser.Text.Trim().Replace(" ", "");
            string dotnetVersion = versionCombo.SelectedItem?.ToString().Trim().Replace(" ", "");

            if (string.IsNullOrEmpty(projectName) ||
                    string.IsNullOrEmpty(location) ||
                    string.IsNullOrEmpty(dotnetVersion))
            {
                MessageBox.Show("Please enter the required information.");
                return;
            }

            progressBar1.Visible = true;


            await Task.Run(() =>
            {
                CreateProject(projectName, location, dotnetVersion);
            });

            textName.Text = "";
            fileLocationBrowser.Text = "Select Project Location";
            versionCombo.SelectedItem = null;

            progressBar1.Visible = false;
            MessageBox.Show($"The project named {projectName} has been created.");

        }
        private void CreateProject(string projectName, string location, string dotnetVersion)
        {
            // dosya işlemleri ve RunDotnet
            string rootPath = Path.Combine(location, projectName);
            string corePath = Path.Combine(rootPath, "Core");
            string infrastructurePath = Path.Combine(rootPath, "Infrastructure");
            string presentationPath = Path.Combine(rootPath, "Presentation");


            Directory.CreateDirectory(corePath);
            Directory.CreateDirectory(infrastructurePath);
            Directory.CreateDirectory(presentationPath);

            //Domain Klasorleri
            Directory.CreateDirectory(Path.Combine(corePath, $"{projectName}.Domain", "Entities"));
            Directory.CreateDirectory(Path.Combine(corePath, $"{projectName}.Domain", "Interfaces"));
            Directory.CreateDirectory(Path.Combine(corePath, $"{projectName}.Domain", "Enums"));

            //Application Klasorleri
            Directory.CreateDirectory(Path.Combine(corePath, $"{projectName}.Application", "CQRS"));
            Directory.CreateDirectory(Path.Combine(corePath, $"{projectName}.Application", "DTOs"));
            Directory.CreateDirectory(Path.Combine(corePath, $"{projectName}.Application", "Mapping"));
            Directory.CreateDirectory(Path.Combine(corePath, $"{projectName}.Application", "Repositories"));
            Directory.CreateDirectory(Path.Combine(corePath, $"{projectName}.Application", "ServiceExtensions"));
            Directory.CreateDirectory(Path.Combine(corePath, $"{projectName}.Application", "Services"));
            Directory.CreateDirectory(Path.Combine(corePath, $"{projectName}.Application", "UnitOfWorks"));
            Directory.CreateDirectory(Path.Combine(corePath, $"{projectName}.Application", "Validations"));

            //Infrastructure Klasorleri
            Directory.CreateDirectory(Path.Combine(infrastructurePath, $"{projectName}.Infrastructure", "Interfaces"));
            Directory.CreateDirectory(Path.Combine(infrastructurePath, $"{projectName}.Infrastructure", "ServiceExtensions"));
            Directory.CreateDirectory(Path.Combine(infrastructurePath, $"{projectName}.Infrastructure", "Services"));

            //Persistence Klasorleri
            Directory.CreateDirectory(Path.Combine(infrastructurePath, $"{projectName}.Persistence", "Configurations"));
            Directory.CreateDirectory(Path.Combine(infrastructurePath, $"{projectName}.Persistence", "DbContext"));
            Directory.CreateDirectory(Path.Combine(infrastructurePath, $"{projectName}.Persistence", "Repositories"));
            Directory.CreateDirectory(Path.Combine(infrastructurePath, $"{projectName}.Persistence", "ServiceExtensions"));
            Directory.CreateDirectory(Path.Combine(infrastructurePath, $"{projectName}.Persistence", "Services"));
            Directory.CreateDirectory(Path.Combine(infrastructurePath, $"{projectName}.Persistence", "UnitOfWorks"));



            RunDotnet($"new classlib -n {projectName}.Domain --framework net{dotnetVersion}", corePath);
            RunDotnet($"new classlib -n {projectName}.Application --framework net{dotnetVersion}", corePath);
            RunDotnet($"new classlib -n {projectName}.Infrastructure --framework net{dotnetVersion}", infrastructurePath);
            RunDotnet($"new classlib -n {projectName}.Persistence --framework net{dotnetVersion}", infrastructurePath);
            RunDotnet($"new webapi -n {projectName}.API --framework net{dotnetVersion}", presentationPath);
            RunDotnet($"new sln -n {projectName}", rootPath);

            RunDotnet($"sln add {corePath}\\{projectName}.Domain\\{projectName}.Domain.csproj", rootPath);
            RunDotnet($"sln add {corePath}\\{projectName}.Application\\{projectName}.Application.csproj", rootPath);
            RunDotnet($"sln add {infrastructurePath}\\{projectName}.Infrastructure\\{projectName}.Infrastructure.csproj", rootPath);
            RunDotnet($"sln add {infrastructurePath}\\{projectName}.Persistence\\{projectName}.Persistence.csproj", rootPath);
            RunDotnet($"sln add {presentationPath}\\{projectName}.API\\{projectName}.API.csproj", rootPath);

            RunDotnet($"add {corePath}\\{projectName}.Application\\{projectName}.Application.csproj reference {corePath}\\{projectName}.Domain\\{projectName}.Domain.csproj", rootPath);
            RunDotnet($"add {infrastructurePath}\\{projectName}.Infrastructure\\{projectName}.Infrastructure.csproj reference {corePath}\\{projectName}.Application\\{projectName}.Application.csproj", rootPath);
            RunDotnet($"add {infrastructurePath}\\{projectName}.Infrastructure\\{projectName}.Infrastructure.csproj reference {corePath}\\{projectName}.Domain\\{projectName}.Domain.csproj", rootPath);
            RunDotnet($"add {infrastructurePath}\\{projectName}.Persistence\\{projectName}.Persistence.csproj reference {infrastructurePath}\\{projectName}.Infrastructure\\{projectName}.Infrastructure.csproj", rootPath);
            RunDotnet($"add {infrastructurePath}\\{projectName}.Persistence\\{projectName}.Persistence.csproj reference {corePath}\\{projectName}.Domain\\{projectName}.Domain.csproj", rootPath);
            RunDotnet($"add {presentationPath}\\{projectName}.API\\{projectName}.API.csproj reference {corePath}\\{projectName}.Application\\{projectName}.Application.csproj", rootPath);
            RunDotnet($"add {presentationPath}\\{projectName}.API\\{projectName}.API.csproj reference {infrastructurePath}\\{projectName}.Infrastructure\\{projectName}.Infrastructure.csproj", rootPath);
        }


        private void RunDotnet(string arguments, string workingDirectory)
        {
            var psi = new ProcessStartInfo("dotnet", arguments)
            {
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(psi))
            {
                process.WaitForExit();
            }
        }


        private void fileLocationBrowser_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    fileLocationBrowser.Text = fbd.SelectedPath;
                }
            }
        }


    }
}
