# -*- coding: utf-8 -*-


import numpy as np
import librosa as lib
import matplotlib.pyplot as plt

def Hanning(N):
    n = np.arange(0,N,1)
    return 0.5 + 0.5 * np.cos(2*np.pi * (n+N/2) / (N - 1))

music_filePath = "07 Summit (feat_ Ellie Goulding).wav"
music_file, sr = lib.load(music_filePath, sr=None)


#block = music_file.shape[0]/sr
#print(block)
music_file_fft = np.fft.fft(music_file[44100:44612]* Hanning(len(music_file[44100:44612])))

print(music_file[44100:44612].shape)
print(music_file_fft.shape)

#print(Hanning(len(music_file[44100:44612])))
#plt.plot(Hanning(len(music_file[44100:44612])))
#plt.plot(music_file[44100:44612])
#plt.plot(music_file[44100:44612] * Hanning(len(music_file[44100:44612])))
plt.plot(np.linspace(0,sr/2,int(len(music_file_fft)/2)), 2*np.abs(music_file_fft[0:int(len(music_file_fft)/2)])/len(music_file_fft))

plt.xlim([0,5000])
plt.show()

