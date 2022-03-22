import socket
import threading
import time
import os

HOST = '10.0.2.2'
PORT = 24


def socketreceiving(ss):
    print ('--start receiving');
    while True:
        time.sleep(0.01);
        data = ss.recv(1024);
        if not data:
            break;
        
        if (data == "gmapping"):
            os.system("sh gmapping.sh &");
            print 'Received:' + data;
        elif (data == "terminal"):
            os.system("sh terminal.sh &");
            print 'Received:' + data;
        else:
            print 'Unknown Received:' + data;

def linuxliving(ss):
    print ('--start sending linuxliving');
    while True:
        ss.send("linuxliving");
        time.sleep(2);


if __name__ == '__main__':
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM);
    s.connect((HOST, PORT));
    sthread = threading.Thread(target=socketreceiving, args=(s,));
    sthread.setDaemon(True);
    sthread.start();
    sthreadlinuxliving = threading.Thread(target=linuxliving, args=(s,));
    sthreadlinuxliving.setDaemon(True)
    sthreadlinuxliving.start();
    while True:
        time.sleep(0.01);
        pass;
