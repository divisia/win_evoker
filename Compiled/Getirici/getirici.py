import shutil
import os

username = os.getlogin()




hedef= "C:/Users/"+username+"/Desktop"
hedef2 = "C:/Users/"+username+"/Desktop"
hedef3 = "C:/Users/"+username+"/Desktop/kisayol"


for ky,ki,di in os.walk(hedef3):

    for x in di:

        if(x.endswith(".lnk") or x.endswith(".zip") or x.endswith(".rar") or x.endswith(".jpg") or x.endswith("png")):

            try:

                shutil.move(hedef3+"/"+x,hedef)

            except:

                pass
os.rmdir(hedef+"/kisayol")