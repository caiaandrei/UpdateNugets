using System.ComponentModel;
using System.IO;
using System.Linq;
using Xunit;

namespace UpdateNugets.Core.Tests
{
    public class ManageNugetsTest
    {
        private string _file2Path;
        private string _file1Path;
        private string _file3Path;

        [Fact]
        public void GetAllNugetsWhenYouHaveUniqeNugetsVersion()
        {
            //Arange
            CreateFile1();

            //Act
            var manageNugets = new WorkspaceNuGetsManager(".", "");

            //Assert
            var nuget = manageNugets.NuGets.FirstOrDefault(item => item.Name.Equals("Castle.Core"));
            Assert.Equal("4.4.1", nuget.Versions[0].NuGetVersion);
            Assert.Equal(".\\file1.csproj", nuget.Versions[0].Files[0]);
        }

        [Fact]
        public void GetAllNugetsWhenYouHaveOneNugetsWithTwoVersions()
        {
            //Arange
            CreateFile1();
            CreateFile2();

            //Act
            var manageNugets = new WorkspaceNuGetsManager(".", "");

            //Assert
            var nuget = manageNugets.NuGets.FirstOrDefault(item => item.Name.Equals("Moq"));
            Assert.Equal("4.14.6", nuget.Versions[0].NuGetVersion);
            Assert.Equal(".\\file1.csproj", nuget.Versions[0].Files[0]);
            Assert.Equal("4.14.5", nuget.Versions[1].NuGetVersion);
            Assert.Equal(".\\file2.csproj", nuget.Versions[1].Files[0]);
        }

        [Fact]
        public void GetAllNugetsWhenYouHaveOneNugetsWithThreeVersions()
        {
            //Arange
            CreateFile1();
            CreateFile2();
            CreateFile3();

            //Act
            var manageNugets = new WorkspaceNuGetsManager(".", "");

            //Assert
            var nuget = manageNugets.NuGets.FirstOrDefault(item => item.Name.Equals("Moq"));
            Assert.Equal("4.14.7", nuget.Versions[2].NuGetVersion);
            Assert.Equal(".\\file3.csproj", nuget.Versions[2].Files[0]);
        }

        private void CreateFile1()
        {
            var createdFile = File.Create("file1.csproj");
            _file1Path = createdFile.Name;
            createdFile.Close();
            File.WriteAllText(_file1Path, "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net5.0</TargetFramework></PropertyGroup><ItemGroup><PackageReference Include=\"Moq\" Version=\"4.14.6\" /><PackageReference Include=\"Castle.Core\" Version=\"4.4.1\" /><PackageReference Include=\"Newtonsoft.Json\" Version=\"12.0.3\" /></ItemGroup></Project>");
        }

        private void CreateFile2()
        {
            var createdFile = File.Create("file2.csproj");
            _file2Path = createdFile.Name;
            createdFile.Close();
            File.WriteAllText(_file2Path, "<?xml version =\"1.0\" encoding=\"utf-8\"?><Project ToolsVersion=\"15.0\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\"><Import Project=\"$(MSBuildExtensionsPath)\\$(MSBuildToolsVersion)\\Microsoft.Common.props\" Condition=\"Exists('$(MSBuildExtensionsPath)\\$(MSBuildToolsVersion)\\Microsoft.Common.props')\" /><PropertyGroup><Configuration Condition=\" '$(Configuration)' == '' \">Debug</Configuration><Platform Condition=\" '$(Platform)' == '' \">AnyCPU</Platform><ProjectGuid>{CC9BFA5B-443F-4368-A423-1751A4274F19}</ProjectGuid><OutputType>Library</OutputType><AppDesignerFolder>Properties</AppDesignerFolder><RootNamespace>VersionAsAChildTesting</RootNamespace><AssemblyName>VersionAsAChildTesting</AssemblyName><TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion><FileAlignment>512</FileAlignment><Deterministic>true</Deterministic></PropertyGroup><PropertyGroup Condition=\" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' \"><DebugSymbols>true</DebugSymbols><DebugType>full</DebugType><Optimize>false</Optimize><OutputPath>bin\\Debug\\</OutputPath><DefineConstants>DEBUG;TRACE</DefineConstants><ErrorReport>prompt</ErrorReport><WarningLevel>4</WarningLevel></PropertyGroup><PropertyGroup Condition=\" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' \"><DebugType>pdbonly</DebugType><Optimize>true</Optimize><OutputPath>bin\\Release\\</OutputPath><DefineConstants>TRACE</DefineConstants><ErrorReport>prompt</ErrorReport><WarningLevel>4</WarningLevel></PropertyGroup><ItemGroup><Reference Include=\"System\" /><Reference Include=\"System.Core\" /><Reference Include=\"System.Xml.Linq\" /><Reference Include=\"System.Data.DataSetExtensions\" /><Reference Include=\"Microsoft.CSharp\" /><Reference Include=\"System.Data\" /><Reference Include=\"System.Net.Http\" /><Reference Include=\"System.Xml\" /></ItemGroup><ItemGroup><Compile Include=\"Class1.cs\" /><Compile Include=\"Properties\\AssemblyInfo.cs\" /></ItemGroup><ItemGroup><PackageReference Include=\"Moq\"><Version>4.14.5</Version></PackageReference><PackageReference Include=\"Serilog\"><Version>2.10.0-dev-01240</Version></PackageReference></ItemGroup><Import Project=\"$(MSBuildToolsPath)\\Microsoft.CSharp.targets\" /></Project>");
        }

        private void CreateFile3()
        {
            var createdFile = File.Create("file3.csproj");
            _file3Path = createdFile.Name;
            createdFile.Close();
            File.WriteAllText(_file3Path, "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net5.0</TargetFramework></PropertyGroup><ItemGroup><PackageReference Include=\"Moq\" Version=\"4.14.7\" /><PackageReference Include=\"Castle.Core\" Version=\"4.4.1\" /><PackageReference Include=\"Newtonsoft.Json\" Version=\"12.0.3\" /></ItemGroup></Project>");
        }
    }
}
