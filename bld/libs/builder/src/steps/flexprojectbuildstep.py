from steps.abstractstep import *
import distutils.dir_util
from util import *
import shutil

class FlexProjectBuildStep(AbstractStep):
    """Flex Project Build Step"""

    def __init__( self, projectPath, libraryPath, assetsPath, outputPath):
        AbstractStep.__init__( self, "Flex Project Build" )
        
	mxmlcBuildPath = "C:\\Program Files\\flex\\bin\\mxmlc.exe"
        #mxmlcBuildPath = "C:\\Program Files (x86)\\Adobe\\Adobe Flash Builder 4\\sdks\\4.1.0\\bin\\mxmlc.exe"
        #self.command = "\"%s\" -locale pt_BR -services \"P:\\gasrd\\Web\\products\\ConsoleHda\\trunk\\src\\flex\\src\\services-config.xml\" -library-path+=\"%s\" -o \"%s\" \"%s\"" % ( mxmlcBuildPath, libraryPath, outputPath + "\\index.swf", projectPath )

	self.command = "\"%s\" -locale pt_BR -library-path+=\"%s\" -o \"%s\" \"%s\"" % ( mxmlcBuildPath, libraryPath, outputPath + "\\index.swf", projectPath )
	
        self.outputPath = outputPath
        self.assetsPath = assetsPath
	
        self.projectPath = projectPath

    def do( self ):
        self.reporter.message( "BUILD FLEX PROJECT: %s" % self.projectPath )
        if (self.outputPath) :
	    distutils.dir_util.copy_tree( self.assetsPath , self.outputPath + "\\assets" )
            #StCommon.CopyTree( self.assetsPath , self.outputPath + "\\assets" )
        return ExecProg( self.command, self.reporter ) == 0
