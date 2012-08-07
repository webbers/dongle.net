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

class WebUtilsFileFilter(FileFilter):
    def __init__( self ):
        FileFilter.__init__( self, ['*\\.svn\\*', '\\uploads' ], ['*Test*', '*Compiler*', 'Ploe*', 'Moq*', 'System*', 'mscorlib*', '*esults*' ], ['.pdb','.nlp']  )



projectRootDir = os.path.abspath( os.path.join( os.path.dirname( __file__ ), '../' ) )
assemblyPath = os.path.abspath( os.path.join( os.path.dirname( __file__ ), '../src/WebUtils/Properties/AssemblyInfo.cs' ) )
pubDir = os.path.abspath( os.path.join( os.path.dirname( __file__ ), '../pub' ) )
scriptDir = os.path.abspath( os.path.join( os.path.dirname( __file__ ), '../src/WebUtils/Scripts/WebUtils' ) )
tempDir = os.path.abspath( os.path.join( os.path.dirname( __file__ ), '../temp' ) )
resDir = os.path.abspath( os.path.join( os.path.dirname( __file__ ), '../res' ) )
repoUrl = 'http://cronos:9090/gasrd/Web/pub/Dongle.Net/trunk'

#--------------------------------------------------------------------
bp = Builder( "Dongle.Net" )
bp.addStep( IncrementBuildVersionStep( assemblyPath, projectRootDir ) )
bp.addStep( MainBuild() )

bp.addStep( SvnDeleteStep(repoUrl))
bp.addStep( DelTreeStep( pubDir ) )
bp.addStep( SvnUpdateDirStep( projectRootDir ) )

bp.addStep( DelTreeStep( tempDir + '\\WebUtils\\testresults' ) )  
bp.addStep( CopyFilteredFilesStep( WebUtilsFileFilter(), tempDir + "\\WebUtils", pubDir ) )
bp.addStep( DelTreeStep( tempDir ) ) 
bp.addStep( SvnImportDirStep( pubDir, repoUrl ) )

if not bp.build():
    sys.exit(1)