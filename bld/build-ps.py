import os
import sys

from mainbuild import *
from steps.checkunversionedfilesstep import *
from steps.tortoisesvncommitstep import *
from steps.deltreestep import *

import re

#--------------------------------------------------------------------
bp = Builder( "Dongle" )

rootPath = os.path.join( os.path.dirname( __file__ ), '../' )
tempDir  = os.path.join( os.path.dirname( __file__ ), '../temp' )

bp.addStep( CheckUnversionedFilesStep( rootPath ) )
bp.addStep( MainBuild() )
bp.addStep( TortoiseSvnCommitStep( rootPath ) )
bp.addStep( DelTreeStep( tempDir ) )  

if not bp.build():
    sys.exit(1)