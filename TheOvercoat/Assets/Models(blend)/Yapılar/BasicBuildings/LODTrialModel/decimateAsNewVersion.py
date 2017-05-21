import bpy
import os

##Cleans all decimate modifiers
def cleanAllDecimateModifiers(obj):
	for m in obj.modifiers:
		if(m.type=="DECIMATE"):
#			print("Removing modifier ")
			obj.modifiers.remove(modifier=m)


numberOfIteration=3
decimateRatio=0.40
modifierName='DecimateMod'
duplicateNotation="_LOD"

for i in range(0,numberOfIteration):
	objectList=bpy.data.objects
	for obj in objectList:
		if(obj.type=="MESH"):

			
			cleanAllDecimateModifiers(obj)

			modifier=obj.modifiers.new(modifierName,'DECIMATE')
			modifier.ratio=1-decimateRatio*(i)
			modifier.use_collapse_triangulate=True

	fileName=bpy.data.filepath
	fileName=os.path.splitext(fileName)[0]

	#Trim decimate version of file name
	indexOf_=fileName.rfind(duplicateNotation[0])
	print(fileName[indexOf_:-len(duplicateNotation)+3])
	if(indexOf_!=-1 and fileName[indexOf_:indexOf_+len(duplicateNotation)]==duplicateNotation):
		fileName=fileName.split(duplicateNotation)[0]


	fileName='{}{}{}{}'.format(fileName,duplicateNotation,str(i+1),".blend")

	bpy.ops.wm.save_as_mainfile(filepath=fileName)
