import os
import sys
import re

from steps.abstractstep import *
from util import *

class IncrementMinorVersionStep(AbstractStep):

    def __init__( self, assemblyPath, projectRoot ):
        AbstractStep.__init__( self, "Set Version Step" )
        self.assemblyPath = assemblyPath
        self.projectRoot = projectRoot

    def do( self ):
        process = subprocess.Popen( "svnversion", cwd=self.projectRoot, stdout=subprocess.PIPE )
        svnrevision = re.search( '\d+', process.stdout.readline() ).group(0)
        
        f = open(self.assemblyPath, 'r')
        content = f.read()
        f.close()
        
        version = re.search( '"(\d+)\.(\d+)\.(\d+)\.(\d+)"', content )
        
        major = version.group(1);
        minor = int(version.group(2)) + 1;
        revision = "0";
        build = svnrevision;
        
        old = version.group(0)
        new = '"' + major + "." + str(minor) + "." + revision + "." + build + '"'
        
        content = content.replace(old,new)
        
        f = open(self.assemblyPath, 'w')
        print >> f, content
        
        return 1