using System;
using System.Globalization;
using System.IO;
using System.Linq;
using DotNetTts.Core;
using DotNetTts.Imp;
using PiperDotNetTts.Imp;
using DotNetTts.Helpers;
using Xunit;

namespace PiperDotNetTtsTest
{

    public class HelpersWhould
    {
        public HelpersWhould()
        {
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData(".temp")]
        public void TempDirectoryInstanceReturnValidDirectoryPath(string extension)
        {
            using TempDirectory d = TempDirectory.Create(extension);
            d.Directory.Create();
            Assert.True(d.Directory.Exists);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData(".temp")]
        public void TempDirectoryDisposedRemoveDirectoryPath(string extension)
        {
            TempDirectory d = TempDirectory.Create(extension);
            d.Directory.Create();
            d.Dispose();
            Assert.True(!d.Directory.Exists);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData(".temp")]
        public void TempFileInstanceReturnValidDirectoryPath(string extension)
        {
            using TempFile d = TempFile.Create(extension);
            d.File.Create();
            Assert.True(d.File.Exists);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData(".temp")]
        public void TempFileDisposedRemoveDirectoryPath(string extension)
        {
            TempFile d = TempFile.Create(extension);
            d.File.Create();
            d.Dispose();
            Assert.True(!d.File.Exists);
        }
        
        [Fact]
        public void CmdExecuteReturnDir()
        {
            if(Environment.OSVersion.Platform.ToString().StartsWith("Win"))
                Cmd.Execute("dir", "c:\\");
            else
                Cmd.Execute("ls", "/");
            
            Assert.True(true);
        }
        
        [Fact]
        public void CmdExecuteShellReturnDir()
        {
            TempFile t = TempFile.Create();
            if(Environment.OSVersion.Platform.ToString().StartsWith("Win"))
                Cmd.ExecuteShell("dir", $"c:\\ > '{t.File.FullName}'");
            else
                Cmd.ExecuteShell("ls", $"/ > '{t.File.FullName}'");
            
            t.File.Refresh();
            Assert.True(t.File.Exists);
        }
        
        [Theory]
        [InlineData("age", true)]
        [InlineData("name", true)]
        [InlineData("width", false)]
        public void BaseReadOnlyPropertiesContainsKeyReturnIfContainsKey(string key, bool contains)
        {
            BaseReadOnlyProperties p = new BaseReadOnlyProperties(new Dictionary<string, object>()
            {
                {"age", 10},
                {"name", "pepe"},
                {"married", false}
            });
            Assert.Equal(contains, p.ContainsKey(key));
        }
        
        [Theory]
        [InlineData("age", typeof(Int32))]
        [InlineData("name", typeof(string))]
        [InlineData("married", typeof(bool))]
        public void BaseReadOnlyPropertiesGetTypeReturnTypeOfProperty(string key, Type type)
        {
            BaseReadOnlyProperties p = new BaseReadOnlyProperties(new Dictionary<string, object>()
            {
                {"age", 10},
                {"name", "pepe"},
                {"married", false}
            });
            Assert.Equal(p.GetType(key), type);
        }
        
        [Fact]
        public void BaseReadOnlyPropertiesKeysReturnKeys()
        {
            BaseReadOnlyProperties p = new BaseReadOnlyProperties(new Dictionary<string, object>()
            {
                {"age", 10},
                {"name", "pepe"},
                {"married", false}
            });
            Assert.NotEmpty(p.Keys);
        }
        
        [Fact]
        public void BaseReadOnlyPropertiesGetValueReturnExistsValue()
        {
            BaseReadOnlyProperties p = new BaseReadOnlyProperties(new Dictionary<string, object>()
            {
                {"age", 10},
                {"name", "pepe"},
                {"married", false}
            });
            Assert.NotEmpty(p.GetValue<string>("name"));
        }
        
        [Fact]
        public void BaseReadOnlyPropertiesGetValueThrowErrorWithNonExistsValue()
        {
            BaseReadOnlyProperties p = new BaseReadOnlyProperties(new Dictionary<string, object>()
            {
                {"age", 10},
                {"name", "pepe"},
                {"married", false}
            });
            Assert.Throws<KeyNotFoundException>(()=>p.GetValue<string>("width"));
        }
        
        [Theory]
        [InlineData("name", true)]
        [InlineData("age", true)]
        [InlineData("width", false)]
        public void BaseReadOnlyPropertiesTryGetValueReturnTrueOrFalseWithExistsNonExistsValue(string key, bool result)
        {
            BaseReadOnlyProperties p = new BaseReadOnlyProperties(new Dictionary<string, object>()
            {
                {"age", 10},
                {"name", "pepe"},
                {"married", false}
            });
            object sname;
            Assert.Equal(result, p.TryGetValue(key, out sname));
        }
        
        [Theory]
        [InlineData("name", "pepe")]
        [InlineData("name", true)]
        [InlineData("age", 23)]
        [InlineData("age", 23.5f)]
        [InlineData("age", "23")]
        public void BasePropertiesSetValueChangeForValidValues(string key, object value)
        {
            BaseProperties p = new BaseProperties(new Dictionary<string, object>()
            {
                {"age", 10},
                {"name", "pepe"},
                {"married", false}
            });
    
            p.SetValue(key, value);
            
            Assert.True(true); 
        }
        
        [Theory]
        [InlineData("age", "pepe")]
        public void BasePropertiesSetValueThrowWithNotValidValues(string key, object value)
        {
            BaseProperties p = new BaseProperties(new Dictionary<string, object>()
            {
                {"age", 10},
                {"name", "pepe"},
                {"married", false}
            });
            
            Assert.Throws<TtsPropertiesException>(()=>p.SetValue(key, value)); 
        }
    }
}