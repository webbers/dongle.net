import os
import sys
import re

import setlibspath

from mainbuild import *
from steps.incrementbuildversionstep import *
from steps.svncommitdirstep import *
from steps.svnimportdirstep import *
from steps.runcommandstep import *
from filters.pythonfilefilter import *
from filters.csbinaryfilefiltertests import *
from steps.svndeletestep import *
from steps.movefilesstep import *

class DongleFileFilter(FileFilter):
    def __init__( self ):
        FileFilter.__init__( self, ['*\\.svn\\*', '\\uploads' ], ['*Test*', '*Compiler*', 'Ploe*', 'Moq*', 'System*', 'Microsoft*', 'mscorlib*', '*esults*' ], ['.pdb','.nlp']  )
		
projectRootDir = os.path.abspath( os.path.join( os.path.dirname( __file__ ), '../' ) )

assemblyPath1 = os.path.abspath( os.path.join( os.path.dirname( __file__ ), '../src/Dongle.Web/Properties/AssemblyInfo.cs' ) )
assemblyPath2 = os.path.abspath( os.path.join( os.path.dirname( __file__ ), '../src/Dongle/Properties/AssemblyInfo.cs' ) )

pubDir = os.path.abspath( os.path.join( os.path.dirname( __file__ ), '../pub' ) )
tempDir = os.path.abspath( os.path.join( os.path.dirname( __file__ ), '../temp' ) )
resDir = os.path.abspath( os.path.join( os.path.dirname( __file__ ), '../res' ) )
repoUrl = 'http://cronos:9090/gasrd/Web/pub/Dongle.Net/trunk'

#--------------------------------------------------------------------
bp = Builder( "Dongle.Net" )
bp.addStep( IncrementBuildVersionStep( assemblyPath1, projectRootDir ) )
bp.addStep( IncrementBuildVersionStep( assemblyPath2, projectRootDir ) )
bp.addStep( MainBuild() )

bp.addStep( DelTreeStep( pubDir ) )
bp.addStep( DelTreeStep( tempDir + '\\Dongle\\testresults' ) )  
bp.addStep( CopyFilteredFilesStep( DongleFileFilter(), tempDir + "\\Dongle.Web", pubDir ) )
bp.addStep( CopyFilteredFilesStep( DongleFileFilter(), tempDir + "\\Dongle", pubDir ) )
bp.addStep( DelTreeStep( tempDir ) ) 

bp.addStep( SvnDeleteStep(repoUrl))
bp.addStep( SvnImportDirStep( pubDir, repoUrl ) )

if not bp.build():
    sys.exit(1)