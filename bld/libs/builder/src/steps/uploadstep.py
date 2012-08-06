# coding: latin-1

import os
import sys
import StFtp
import StCommon

from steps.abstractstep import *

class UploadStep(AbstractStep):
    """DelTree Step"""
    def __init__( self, localPath, remoteHost, remoteUser, remotePassword, remotePath ):
        AbstractStep.__init__(self, 'UploadStep')
        self.localPath = localPath
        self.remoteHost = remoteHost
        self.remoteUser = remoteUser
        self.remotePassword = remotePassword
        self.remotePath = remotePath
        self.uploadPath = ''

    def do( self ):
        if ( os.path.exists( self.localPath ) ):
            ftpConnection = StFtp.FtpConnect( self.remoteHost, self.remoteUser, self.remotePassword )

            self.createAppOfflineFile(self.localPath)
            self.reporter.message( 'arquivo de aviso de manutencao criado' )

            if ( self.uploadPath != '' ):
			    if ( StFtp.CheckRemoteDir( ftpConnection, self.uploadPath ) == False ):
					try:
						ftpConnection.mkd( self.uploadPath )
					except:
						self.reporter.message( 'Pasta de upload criada. Defina permissao de escrita no diretorio.' )
						pass

            if ( self.uploadDirStruct( self.localPath, self.remotePath, ftpConnection, True ) ):
                ftpConnection.delete( self.remotePath + '/app_offline.htm' )
                return True
        return False
    
    def createAppOfflineFile( self, destPath ):
        fileHandle = open( destPath + '/app_offline.htm', 'w' )
        fileHandle.write( r'<strong>A aplicação está em manutenção no momento. Tente novamente mais tarde.</strong>' )
        fileHandle.close()
        
    def uploadDirStruct( self, localPath, remotePath, ftpConnection, recursive=False ):
        try:
            if ( ( remotePath != "" ) or ( remotePath != "/" ) ):
                try:
                    ftpConnection.mkd( remotePath )
                except:
                    pass
                
            for fileDir in StCommon.ListFiles( localPath ):
                self.uploadSimpleFile( fileDir, ftpConnection, remotePath )
        
            if ( recursive ):
                for dir in ( StCommon.ListDirs( localPath, 0 ) ):
                    self.uploadDirStruct( ( localPath + "\\" + os.path.basename( dir ) ), ( remotePath + "/" + os.path.basename( dir ) ), ftpConnection, True )
            return True
        except:
            return False
    
    def uploadSimpleFile( self, File, ftpConnection, DstPath  ):
        ftpConnection.cwd( DstPath )
        try:
            FileName = open( File, "rb" )
            NewFile = File.split( "\\" )[-1]
            
            ftpConnection.storbinary( "STOR " + NewFile, FileName )
            self.reporter.success('Arquivo enviado: ' + NewFile)
            Size = ftpConnection.size( NewFile )
            ftpConnection.cwd( "/" )
            return True
        
        except( ftplib.Error, ftplib.socket.error ), Error:
            ftpConnection.cwd( "/" )
            return False
        