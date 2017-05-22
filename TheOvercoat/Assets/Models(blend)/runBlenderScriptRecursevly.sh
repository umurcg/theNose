find $(pwd) -name '*.blend' -exec blender {} --background --python createLOD.py \;
read

