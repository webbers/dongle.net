from steps.abstractstep import *
import zipfile
import os, fnmatch, shutil, time, datetime, sys, UserDict, subprocess, stat

def WalkDir( root, recurse=0, pattern='*', return_folders=0 ):
    import fnmatch, os, string

    result = []

    try:
        names = os.listdir(root)
    except os.error:
        return result

    pattern = pattern or '*'
    pat_list = string.splitfields( pattern , ';' )

    for name in names:
        fullname = os.path.normpath(os.path.join(root, name))

        for pat in pat_list:
            if fnmatch.fnmatch(name, pat):
                if os.path.isfile(fullname) or (return_folders and os.path.isdir(fullname)):
                    result.append(fullname)
                continue

        if recurse:
            if os.path.isdir(fullname) and not os.path.islink(fullname):
                result = result + WalkDir( fullname, recurse, pattern, return_folders )

    return result

def ListFiles( BaseDir, rec=0):
    return WalkDir( BaseDir, rec, '*', 0 )
	
def ClearFileAttributes( fp ):
    import win32file
    return ( win32file.SetFileAttributesW( StUnicode( fp ) , win32file.FILE_ATTRIBUTE_NORMAL ) == win32con.TRUE )
	
def DeleteFile( FilePath ):
    if os.path.exists( FilePath ):
        ClearFileAttributes( FilePath )
        os.remove( FilePath )
    return not os.path.exists( FilePath )

class ZipFilesStep(AbstractStep):
    """Zip files Step"""
    
    def __init__(self, srcDir, destFile):
        AbstractStep.__init__(self, "Zip files")
        
        self.srcDir = srcDir
        self.destFile = destFile
        
    def do( self ):
        files = ListFiles( self.srcDir, True )
        DeleteFile(self.destFile)
        zf = zipfile.ZipFile(self.destFile, 'w')
        result = True
        
        for fp in files:
            try:
                zf.write(fp, fp, zipfile.ZIP_DEFLATED)
            except:
                result = False
        zf.close()
        
        return result