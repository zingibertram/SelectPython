# -*- coding: utf-8 -*-

import sys
import platform as pl


try:
    import PyQt4
    pyqt = 'PyQt4'
except:
    try:
        import PyQt5
        pyqt = 'PyQt5'
    except:
        pyqt = 'No PyQt'
        

ver = "%d.%d.%d//%s//%s" % (sys.version_info[0], sys.version_info[1], sys.version_info[2], pl.architecture()[0], pyqt)
print(ver)
