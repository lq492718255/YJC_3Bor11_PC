dqy=100.7; %����ѹ
x=[ hex2dec('832')  hex2dec('1E3D')]; %�궨�µ�Tvaֵ
y=[ dqy*0.209  dqy*0.75]; %��������ѹ
tva=2233;
P=polyfit(x,y,1)
oxy=polyval(P,tva);
% [P,S]=polyfit(x,y,1);
% [oxy,DELTA]=polyval(P,tva,S) 



