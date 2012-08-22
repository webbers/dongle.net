import os
import sys
import re
import setlibspath

from builder import *
from steps.deltreestep import *
from steps.delstep import *
from steps.svnupdatedirstep import *
from test import *
from steps.csprojectbuildstep import *
from steps.runcsunitteststep import *
from steps.batchfilereplacestep import *
from steps.copyfilteredfilesstep import *
from steps.copydirstep import *
from steps.copyfilestep import *
from steps.movefilesstep import *
from filters.csbinaryfilefilter import *
from filters.docsfilefilter import *

sys.path.insert( 0, os.path.abspath(os.path.join( os.path.dirname( __file__ ), '../src' ) ) )

#--------------------------------------------------------------------
class MainBuild( Builder ):
    def __init__( self ):
        Builder.__init__( self, "Dongle Main Build" )

    def build( self ):
        tempDir  = os.path.join( os.path.dirname( __file__ ), '../temp' )
        consoleTempDir  = os.path.join( os.path.dirname( __file__ ), '../temp/Dongle.Web/' )
        pubDir = os.path.join( os.path.dirname( __file__ ), '../pub' )
        resDir = os.path.join( os.path.dirname( __file__ ), '../res' )
        consoleProjectPath = os.path.join( os.path.dirname( __file__ ), '../src/Dongle.sln' )
		
        self.addStep( DelTreeStep( tempDir ) )        
        self.addStep( SvnUpdateDirStep( os.path.join( os.path.dirname( __file__ ), '../' ) ) )
        self.addStep( CsProjectBuildStep( consoleProjectPath, consoleTempDir ) )        
        self.addStep( RunCsUnitTestStep( consoleTempDir + 'Dongle.Web.Tests.dll' ) )
        
                
        return Builder.build(self)