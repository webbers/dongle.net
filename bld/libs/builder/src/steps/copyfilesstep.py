import os
import shutil

from steps.abstractstep import *

def copyfile(filename1, filename2):
    if not os.path.exists(os.path.dirname(filename2)):
        os.makedirs(os.path.dirname(filename2))
    shutil.copy( filename1, os.path.dirname(filename2) )
    if os.path.isfile (filename2): return True
    return False

class CopyFilesStep(AbstractStep):
    """Copy Files Step"""

    def __init__( self, files, srcDir, destDir ):
        AbstractStep.__init__( self, "Copy Files" )
        self.srcDir = srcDir
        self.destDir = destDir
        self.files = files


    def do( self ):
        self.reporter.message( "COPY FILES: %s => %s" % ( self.srcDir, self.destDir ) )

        for fp in self.files:
            relPath = fp.lower().replace( os.path.realpath( self.srcDir ).lower(), "" )
            destPath = os.path.realpath( self.destDir ) + relPath
            self.reporter.message(fp)
           
            if not copyfile( fp, destPath ):
                self.reporter.failure("copying %s to %s" % (fp, destPath))
                return False  
        return True
    
    def setFiles( self, files ):
        self.files = files