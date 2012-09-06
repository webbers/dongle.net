import os
from steps.abstractstep import *

def removeDir(path):
    AbsPath = os.path.abspath( path )
    if not os.path.exists( AbsPath ):
        return False
    
    if os.name == "nt":
        cmd = """rmdir /S /Q "%s" """ % AbsPath
    else:
        cmd = """rm -rf "%s" """ % AbsPath
	
    os.system( cmd )
    return not ( os.path.exists( AbsPath ) )

class DelTreeStep(AbstractStep):
    """DelTree Step"""

    def __init__( self, srcDir ):
        AbstractStep.__init__(self, "DelTree")
        self.srcDir = srcDir
        
    def do( self ):
        self.reporter.message( "DELTREE: %s" % ( self.srcDir ) ) 

        if ( os.path.exists( self.srcDir ) ):
            #try:   
                removeDir( self.srcDir )            
                return True
            #except:
                print "error"
                return False
        else:
            return True