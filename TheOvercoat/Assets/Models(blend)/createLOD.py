import bpy
import os
import sys

numberOfIteration=2
decimateRatio=0.40
modifierName='DecimateMod'
duplicateNotation="_LOD"

##Cleans all decimate modifiers
def cleanAllDecimateModifiers(obj):
	for m in obj.modifiers:
		if(m.type=="DECIMATE"):
#			print("Removing modifier ")
			obj.modifiers.remove(modifier=m)


fileName=bpy.path.basename(bpy.data.filepath)
fileName=os.path.splitext(fileName)[0]

objectList=bpy.data.objects

#First clear light and camera
for obj in objectList:
	print(obj.type)
	if(obj.type!="MESH"):
		print("Found "+obj.name+" and deleting it")
		obj.select=True
		bpy.ops.object.delete()

objectList=bpy.data.objects

#Now if number of object is more than one then exit
if(len(objectList)!=1):
	 print("There are more than one object. Terminating script.")
	 sys.exit() 

mainObject=objectList[0]

#Change object name to fileName
mainObject.name=fileName+duplicateNotation+str(0)


for i in range(0,numberOfIteration):
 		if(mainObject.type=="MESH"):

 			bpy.ops.object.duplicate()
 			duplicatedObj=bpy.context.selected_objects[0]			
 			cleanAllDecimateModifiers(duplicatedObj)

 			modifier=duplicatedObj.modifiers.new(modifierName,'DECIMATE')
 			modifier.ratio=1-decimateRatio*(i+1)
 			modifier.use_collapse_triangulate=True
 			duplicatedObj.name=fileName+duplicateNotation+str(i+1)

bpy.ops.wm.save_mainfile()

# 			modifier=obj.modifiers.new(modifierName,'DECIMATE')
# 			modifier.ratio=1-decimateRatio*(i)
# 			modifier.use_collapse_triangulate=True

	




# 	fileName='{}{}{}{}'.format(fileName,duplicateNotation,str(i),".blend")

# 	bpy.ops.wm.save_as_mainfile(filepath=fileName)
