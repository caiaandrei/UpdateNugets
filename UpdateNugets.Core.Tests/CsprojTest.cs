using System.IO;
using System.Linq;
using Xunit;

namespace UpdateNugets.Core.Tests
{
    public class CsprojTest
    {
        private string _fileVersionAsAnAttributePath;
        private string _fileVersionAsAChildPath;

        [Fact]
        public void ReadAllNugesWhenTheVersionIsAnAttribute()
        {
            //Arrange
            CreateAttributeFile();
            var csproj = new Csproj(_fileVersionAsAnAttributePath);

            //Act
            var allNugets = csproj.GetAllNugets();

            //Assert
            Assert.Equal("Castle.Core", allNugets.First().Key);
            Assert.Equal("4.4.1", allNugets.First().Value);
        }

        [Fact]
        public void UpdateANugeWhenTheVersionIsAnAttribute()
        {
            //Arrange
            CreateAttributeFile();
            var csproj = new Csproj(_fileVersionAsAnAttributePath);

            //Act
            csproj.UpdateANuget("Castle.Core", "20.20.100");
            var allNugets = csproj.GetAllNugets();

            //Assert
            Assert.Equal("20.20.100", allNugets.First().Value);
        }

        [Fact]
        public void ReadAllNugesWhenTheVersionIsAChild()
        {
            //Arrange
            CreateChildFile();
            var csproj = new Csproj(_fileVersionAsAChildPath);

            //Act
            var allNugets = csproj.GetAllNugets();

            //Assert
            Assert.Equal("Moq", allNugets.First().Key);
            Assert.Equal("4.14.5", allNugets.First().Value);
        }

        [Fact]
        public void UpdateANugeWhenTheVersionIsAChild()
        {
            //Arrange
            CreateChildFile();
            var csproj = new Csproj(_fileVersionAsAChildPath);

            //Act
            csproj.UpdateANuget("Moq", "20.20.100");
            var allNugets = csproj.GetAllNugets();

            //Assert
            Assert.Equal("20.20.100", allNugets.First().Value);
        }

        private void CreateChildFile()
        {
            var createdFile = File.Create("childFile.csproj");
            _fileVersionAsAChildPath = createdFile.Name;
            createdFile.Close();
            File.WriteAllText(_fileVersionAsAChildPath, "<?xml version =\"1.0\" encoding=\"utf-8\"?><Project ToolsVersion=\"15.0\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\"><Import Project=\"$(MSBuildExtensionsPath)\\$(MSBuildToolsVersion)\\Microsoft.Common.props\" Condition=\"Exists('$(MSBuildExtensionsPath)\\$(MSBuildToolsVersion)\\Microsoft.Common.props')\" /><PropertyGroup><Configuration Condition=\" '$(Configuration)' == '' \">Debug</Configuration><Platform Condition=\" '$(Platform)' == '' \">AnyCPU</Platform><ProjectGuid>{CC9BFA5B-443F-4368-A423-1751A4274F19}</ProjectGuid><OutputType>Library</OutputType><AppDesignerFolder>Properties</AppDesignerFolder><RootNamespace>VersionAsAChildTesting</RootNamespace><AssemblyName>VersionAsAChildTesting</AssemblyName><TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion><FileAlignment>512</FileAlignment><Deterministic>true</Deterministic></PropertyGroup><PropertyGroup Condition=\" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' \"><DebugSymbols>true</DebugSymbols><DebugType>full</DebugType><Optimize>false</Optimize><OutputPath>bin\\Debug\\</OutputPath><DefineConstants>DEBUG;TRACE</DefineConstants><ErrorReport>prompt</ErrorReport><WarningLevel>4</WarningLevel></PropertyGroup><PropertyGroup Condition=\" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' \"><DebugType>pdbonly</DebugType><Optimize>true</Optimize><OutputPath>bin\\Release\\</OutputPath><DefineConstants>TRACE</DefineConstants><ErrorReport>prompt</ErrorReport><WarningLevel>4</WarningLevel></PropertyGroup><ItemGroup><Reference Include=\"System\" /><Reference Include=\"System.Core\" /><Reference Include=\"System.Xml.Linq\" /><Reference Include=\"System.Data.DataSetExtensions\" /><Reference Include=\"Microsoft.CSharp\" /><Reference Include=\"System.Data\" /><Reference Include=\"System.Net.Http\" /><Reference Include=\"System.Xml\" /></ItemGroup><ItemGroup><Compile Include=\"Class1.cs\" /><Compile Include=\"Properties\\AssemblyInfo.cs\" /></ItemGroup><ItemGroup><PackageReference Include=\"Moq\"><Version>4.14.5</Version></PackageReference><PackageReference Include=\"Serilog\"><Version>2.10.0-dev-01240</Version></PackageReference></ItemGroup><Import Project=\"$(MSBuildToolsPath)\\Microsoft.CSharp.targets\" /></Project>");
        }

        private void CreateAttributeFile()
        {
            var createdFile = File.Create("attributeFile.csproj");
            _fileVersionAsAnAttributePath = createdFile.Name;
            createdFile.Close();
            File.WriteAllText(_fileVersionAsAnAttributePath, "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net5.0</TargetFramework></PropertyGroup><ItemGroup><PackageReference Include=\"Castle.Core\" Version=\"4.4.1\" /><PackageReference Include=\"Newtonsoft.Json\" Version=\"12.0.3\" /></ItemGroup></Project>");
        }
    }
}
