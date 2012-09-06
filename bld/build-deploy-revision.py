import os
import sys
import re

import setlibspath

from mainbuild import *
from steps.incrementrevisionversionstep import *
from steps.svncommitdirstep import *
from steps.svnimportdirstep import *
from steps.svncreatetagdirstep import *
from steps.runcommandstep import *
from filters.pythonfilefilter import *
from filters.csbinaryfilefiltertests import *
from steps.svndeletestep import *
from steps.movefilesstep import *

class DongleFileFilter(FileFilter):
    def __init__( self ):
        FileFilter.__init__( self, ['*\\.svn\\*', '\\uploads' ], ['*Test*', '*Compiler*', 'Ploe*', 'Moq*', 'System*', 'mscorlib*', '*esults*' ], ['.pdb','.nlp']  )

projectRootDir = os.path.abspath( os.path.join( os.path.dirname( __file__ ), '../' ) )
assemblyPath1 = os.path.abspath( os.path.join( os.path.dirname( __file__ ), '../src/Dongle/Properties/AssemblyInfo.cs' ) )
assemblyPath2 = os.path.abspath( os.path.join( os.path.dirname( __file__ ), '../src/Dongle.Web/Properties/AssemblyInfo.cs' ) )

assemblyDir1 = os.path.abspath( os.path.join( os.path.dirname( __file__ ), '../src/Dongle/Properties/' ) )
assemblyDir2 = os.path.abspath( os.path.join( os.path.dirname( __file__ ), '../src/Dongle.Web/Properties/' ) )

pubDir = os.path.abspath( os.path.join( os.path.dirname( __file__ ), '../pub' ) )
tempDir = os.path.abspath( os.path.join( os.path.dirname( __file__ ), '../temp' ) )
repoUrl = 'http://cronos:9090/gasrd/Web/pub/Dongle.Net/trunk'

#--------------------------------------------------------------------
bp = Builder( "Dongle.Net" )
bp.addStep( IncrementRevisionVersionStep( assemblyPath1, projectRootDir ) )
bp.addStep( IncrementRevisionVersionStep( assemblyPath2, projectRootDir ) )
bp.addStep( MainBuild() )

bp.addStep( DelTreeStep( pubDir ) )
bp.addStep( DelTreeStep( tempDir + '\\Dongle\\testresults' ) )  

bp.addStep( CopyFilteredFilesStep( DongleFileFilter(), tempDir + "\\Dongle.Web", pubDir ) )
bp.addStep( CopyFilteredFilesStep( DongleFileFilter(), tempDir + "\\Dongle", pubDir ) )
bp.addStep( DelTreeStep( tempDir ) ) 

bp.addStep( SvnDeleteStep(repoUrl))
bp.addStep( SvnImportDirStep( pubDir, repoUrl ) )
bp.addStep( SvnCreateTagDirStep( repoUrl,  'http://cronos:9090/gasrd/Web/pub/Dongle.Net/tags', assemblyPath1 ) )

bp.addStep( SvnCommitDirStep( assemblyDir1, 1, sys.argv[1], sys.argv[2]  ) )

if not bp.build():
    sys.exit(1)