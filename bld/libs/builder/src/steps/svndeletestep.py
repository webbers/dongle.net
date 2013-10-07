from steps.abstractstep import *
from util import *

def execProg(command):
    startupinfo = subprocess.STARTUPINFO()
    startupinfo.dwFlags |= subprocess.STARTF_USESHOWWINDOW
    process = subprocess.Popen( command, stdout=subprocess.PIPE, startupinfo=startupinfo )
    list=[]
    for line in process.stdout.readlines():
        list.append(line.replace('\r\n',''))
    if process.wait() == 0:
        return list
    return -1

def svnList(path, reporter):
    list = execProg('svn list ' + path)
    if list == -1:
        printError('Erro ao ler o caminho: ' + path)
        return []
    return list

class SvnDeleteStep(AbstractStep):
    """Svn Delete Step"""

    def __init__( self, dir ):
        AbstractStep.__init__( self, "Svn Delete Dir" )
        self.dir = dir

    def do( self ):
        self.reporter.message( "DELETE: %s" % self.dir )
        
        for path in svnList(self.dir, self.reporter):
            command = "svn delete --non-interactive --trust-server-cert --force \"%s\" -m \"Build\"" % (self.dir + "/" + path)
            print command
            ExecProg( command, self.reporter ) == 0
        return 1
