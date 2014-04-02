module.exports = function(grunt) {
  require('load-grunt-tasks')(grunt);
  require('time-grunt')(grunt);

  grunt.initConfig({
  
    pkg: grunt.file.readJSON('package.json'),
  
    nugetpush: {
        default: {
            src: 'pub/<%= pkg.name %>.<%= pkg.version %>.0.nupkg'
        },
        web: {
            src: 'pub/<%= pkg.name %>.Web.<%= pkg.version %>.0.nupkg'
        },
        data: {
            src: 'pub/<%= pkg.name %>.Data.<%= pkg.version %>.0.nupkg'
        }
    },
    
    shell: {
        nugetpack: {
            options: {
                stdout: true
            },
            command: 'md pub & nuget pack src/Dongle/Dongle.csproj -Prop Configuration=Release -OutputDirectory pub'
        },
        nugetpack_web: {
            options: {
                stdout: true
            },
            command: 'md pub & nuget pack src/Dongle.Web/Dongle.Web.csproj -Prop Configuration=Release -OutputDirectory pub'
        },
        nugetpack_data: {
            options: {
                stdout: true
            },
            command: 'md pub & nuget pack src/Dongle.Data/Dongle.Data.csproj -Prop Configuration=Release -OutputDirectory pub'
        },
        gitcommit: {
            options: {
                stdout: true
            },
            command: 'git commit -a -m "version <%= pkg.version %>'
        }
    },
    
    assemblyinfo: {
        options: {
            files: ['src/Dongle/Dongle.csproj', 'src/Dongle.Web/Dongle.Web.csproj', 'src/Dongle.Data/Dongle.Data.csproj'],
            info: {
                description: '<%= pkg.description %>', 
                configuration: 'Release', 
                company: '<%= pkg.author %>', 
                product: '<%= pkg.name %>', 
                copyright: 'Copyright © <%= pkg.author %> ' + (new Date().getYear() + 1900), 
                version: '<%= pkg.version %>.0', 
                fileVersion: '<%= pkg.version %>.0'
            }
        }
    },
    
    msbuild: {
        src: ['src/Dongle.sln'],
        options: {
            verbosity: 'minimal',
            projectConfiguration: 'Release',
            targets: ['Clean', 'Rebuild'],
            stdout: true
        }
    },
    
    nunit: {
        options: {
            files: ['src/Dongle.Tests/bin/Release/Dongle.Tests.dll', 'src/Dongle.Web.Tests/bin/Release/Dongle.Web.Tests.dll']
        }
    }
    
  });
  grunt.registerTask('nugetpack', ['shell:nugetpack', 'shell:nugetpack_web', 'shell:nugetpack_data']);
  
  grunt.registerTask('default', ['assemblyinfo', 'msbuild', 'nunit', 'nugetpack']);
  grunt.registerTask('push', ['default', 'nugetpush', 'shell:gitcommit']);
};