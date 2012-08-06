from filters.filefilter import *

class PythonFileFilter(FileFilter):
    def __init__( self ):
        FileFilter.__init__( self, ['*\\.svn\\*', '*\\testdata\\*', '*\\install\\*', '*\\docs\\*', '*\\pub\\*', '*\\libs\\*', "*\\bld\\*" ], ['test_*', '_publish.py', '_test.py' ], ['.pyc', '.wpr'] )
