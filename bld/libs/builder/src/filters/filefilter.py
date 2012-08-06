import os
import fnmatch

def WalkDir( root, recurse=0, pattern='*', return_folders=0 ):
    import fnmatch, os, string

    # initialize
    result = []

    # must have at least root folder
    try:
        names = os.listdir(root)
    except os.error:
        return result

    # expand pattern
    pattern = pattern or '*'
    pat_list = string.splitfields( pattern , ';' )

    # check each file
    for name in names:
        fullname = os.path.normpath(os.path.join(root, name))

        # grab if it matches our pattern and entry type
        for pat in pat_list:
            if fnmatch.fnmatch(name, pat):
                if os.path.isfile(fullname) or (return_folders and os.path.isdir(fullname)):
                    result.append(fullname)
                continue

        # recursively scan other folders, appending results
        if recurse:
            if os.path.isdir(fullname) and not os.path.islink(fullname):
                result = result + WalkDir( fullname, recurse, pattern, return_folders )

    return result

class FileFilter:
    AllPtn = []
    FileNamePtn = []
    ExtPtn = []

    def __init__( self, AllPtn, FileNamePtn, ExtPtn, AllPtnFlag = False, FileNamePtnFlag = False, ExtPtnFlag = False ):
        #Flag True para inclusao e False para exclusao
        self.AllPtn = AllPtn
        self.FileNamePtn = FileNamePtn
        self.ExtPtn = ExtPtn
        self.AllPtnFlag = AllPtnFlag
        self.FileNamePtnFlag = FileNamePtnFlag
        self.ExtPtnFlag = ExtPtnFlag
        
    def Filter( self, Dir, Recurvise = True ):
        Files = WalkDir( Dir, Recurvise )
        FiltredFiles = []
        for fp in Files:
            if self.IsValidFile( fp, Dir ):
                FiltredFiles.append( fp )
        return FiltredFiles
    
    def IsValidFile( self, fp, BaseDir ):
        fp = fp.lower().replace( BaseDir.lower(), '' )
        fn = os.path.basename( fp )
        ext = os.path.splitext( fp )[1].lower()
        
        # Retira por path completo (jah sem o BaseDir)
        for ptn in self.AllPtn:
            if fnmatch.fnmatch( fp, ptn ):
                if self.AllPtnFlag:
                    return True
                return False
            
        # Retira por filename
        for ptn in self.FileNamePtn:
            if fnmatch.fnmatch( fn, ptn ):
                if self.FileNamePtnFlag:
                    return True
                return False
            
        # Retira por extensao
        for ptn in self.ExtPtn:
            if fnmatch.fnmatch( ext, ptn ):
                if self.ExtPtnFlag:
                    return True
                return False
            
        if not self.AllPtn:
            return False
        
        if not self.FileNamePtn:
            return False
        
        if not self.ExtPtn:
            return False
        
        return True