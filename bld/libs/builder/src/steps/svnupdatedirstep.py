from steps.abstractstep import *
from util import *

class SvnUpdateDirStep(AbstractStep):
    """Svn Update Dir Step"""

    def __init__( self, dir ):
        AbstractStep.__init__( self, "Svn Update Dir" )
        self.dir = dir

    def do( self ):
        self.reporter.message( "SVN UPDATE DIR: %s" % self.dir )
        return ExecProg( "svn update", self.reporter, self.dir ) == 0