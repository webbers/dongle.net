import os
import sys
import re

from steps.abstractstep import *
from util import *
import distutils.dir_util

class CopyDirStep(AbstractStep):
    """Copy Dir Step"""

    def __init__( self, srcDir, destDir ):
        AbstractStep.__init__( self, "Copy Dir" )
        
        self.srcDir = srcDir
        self.destDir = destDir
        
    def do( self ):
        self.reporter.message( "COPY DIR: %s => %s" % ( self.srcDir, self.destDir ) )        
        return distutils.dir_util.copy_tree( self.srcDir, self.destDir )