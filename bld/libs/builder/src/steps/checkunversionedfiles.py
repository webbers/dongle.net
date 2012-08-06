import os
import StCommon
from StBuild import CStBuild
from Reporters import *
from Steps import *
import re

#--------------------------------------------------------------------
class CheckUnversionedFilesSteps( AbstractStep ):
    def __init__( self, baseDir ):
        AbstractStep.__init__( self, "Check Unversioned Files" )
        self.__baseDir = baseDir

    def build( self ):
        br = BufferedReporter()
        print "svn status " + self.__baseDir
        ExecProg( "svn status " + self.__baseDir, br, self.__baseDir )
        
        files = re.findall('\?\s+([^\n]+)\n', br.getBuffer() )
        for file in files:
            self.reporter.failure( "Arquivo \"" + file + "\" nao versionado" )
            
        if( len(files) ):
            self.reporter.failure( str(len(files)) + " arquivos nao versionados" )
            return 0
            
        return 1

