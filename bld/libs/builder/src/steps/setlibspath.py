import sys, os

def include( file ):
    file = os.path.abspath( os.path.join( os.path.dirname( __file__ ), file ) )
    sys.path.insert( 0, file )
    
include( '../../res/libs/StLibsPy' )