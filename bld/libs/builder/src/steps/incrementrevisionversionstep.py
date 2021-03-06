import os
import sys
import re

from steps.abstractstep import *
from util import *

class IncrementRevisionVersionStep(AbstractStep):

    def __init__( self, assemblyPath, projectRoot ):
        AbstractStep.__init__( self, "Set Version Step" )
        self.assemblyPath = assemblyPath
        self.projectRoot = projectRoot

    def do( self ):
        f = open(self.assemblyPath, 'r')
        content = f.read()
        f.close()
        
        version = re.search( '"(\d+)\.(\d+)\.(\d+)\.(\d+)"', content )
        
        major = version.group(1);
        minor = version.group(2);
        revision = int(version.group(3)) + 1;
        build = 0;
        
        old = version.group(0)
        new = '"' + major + "." + minor + "." + str( revision ) + '.0"'
        
        content = content.replace(old,new)
        
        f = open(self.assemblyPath, 'w')
        print >> f, content
        
        return 1