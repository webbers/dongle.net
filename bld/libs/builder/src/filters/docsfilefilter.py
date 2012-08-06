from filters.filefilter import *

class DocsFileFilter(FileFilter):
    def __init__( self ):
        FileFilter.__init__( self, [], [], ['.doc','.pdf'], True )
