import os
import sys
import io
import pandas as pd
sys.path.insert(0, "libs/")
import tNavigator_python_API as tnav

current_directory=os.getcwd()
os.chdir('BuildNetworkExample/python')
current_directory=os.getcwd()
with open('../dataframe.txt', 'r', encoding='utf-8') as file:
  file_content=file.read()
bytes = bytes.fromhex(file_content)
memory_stream=io.BytesIO(bytes)

df_nd_results = tnav.unpack_data(memory_stream)
print('Done')

print('Creating results folder...', end=' ', flush=True)
new_folder = 'Result_Tables'
if not os.path.exists(new_folder):
    os.makedirs(new_folder)
else:
    print(f"The folder with the `{new_folder}` name already exists!")
print('Done')

print('Saving to file...', end=' ', flush=True)
df_nd_results.to_csv("Result_Tables/pipes_table_results.csv")
print('Done')
print('Surface network is successfully calculated. The script has been finished')