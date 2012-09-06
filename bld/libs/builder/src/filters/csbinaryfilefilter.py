from filters.filefilter import *

class CsBinaryFileFilter(FileFilter):
    def __init__( self ):
        FileFilter.__init__( self, ['*\\.svn\\*', '\\uploads' ], ['*Test.dll', '*Test.dll.config', '*Tests.dll', '*Tests.dll.config', '*Test.exe', '*_accessor.dll' ], ['.pdb','.nlp'] )
