#!bin/bash

#echo ${0%}
rm Documentation.txt

for d in */ ; do
  cat $d/*.txt >> Documentation.txt
done

echo "done"

