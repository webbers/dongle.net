import os
from steps.abstractstep import *

def createDir(path):
    AbsPath = os.path.abspath( path )
    if os.path.exists( AbsPath ):
        return False
    
    if os.name == "nt":
        cmd = """mkdir /S /Q "%s" """ % AbsPath
    else:
        cmd = """md -rf "%s" """ % AbsPath
	
    os.system( cmd )
    return ( os.path.exists( AbsPath ) )

class CreateDirStep(AbstractStep):
    """Create Dir Step"""

    def __init__( self, srcDir ):
        AbstractStep.__init__(self, "Create Dir")
        self.srcDir = srcDir
        
    def do( self ):
        self.reporter.message( "CREATE DIR: %s" % ( self.srcDir ) ) 

        if ( os.path.exists( self.srcDir ) ):
            #try:   
                createDir( self.srcDir )            
                return True
            #except:
                print "error"
                return False
        else:
            return True