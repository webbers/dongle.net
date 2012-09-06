from filters.filefilter import *

class CsBinaryFileFilterTests(FileFilter):
    def __init__( self ):
        FileFilter.__init__( self, ['*\\.svn\\*', '\\uploads' ], ['*Test.dll', '*Test.dll.config', '*Tests.dll', '*Tests.dll.config', '*_accessor.dll' ], ['.pdb','.nlp'] )
