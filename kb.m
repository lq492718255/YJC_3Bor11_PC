dqy=100.7; %大气压
x=[ hex2dec('832')  hex2dec('1E3D')]; %标定下的Tva值
y=[ dqy*0.209  dqy*0.75]; %理论氧分压
tva=2233;
P=polyfit(x,y,1)
oxy=polyval(P,tva);
% [P,S]=polyfit(x,y,1);
% [oxy,DELTA]=polyval(P,tva,S) 



