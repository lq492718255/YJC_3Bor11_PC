clc
clear all
tva=hex2dec('5BE');
dqy=100.7;%100.7; %��ǰ����ѹ
y_5=[ dqy*0.20  dqy*0.209  dqy*0.50  dqy*0.75  dqy*0.95 ]; %��������ѹ
% x_5=[ hex2dec('7AF') hex2dec('800')  hex2dec('1393') hex2dec('1DA8') hex2dec('25D0')]; %5��궨�µ�Tvaֵ,3B
x_5=[ hex2dec('7DC') hex2dec('832')  hex2dec('13F1') hex2dec('1E3D') hex2dec('2684')]; %5��궨�µ�Tvaֵ,11
P_5=polyfit(x_5,y_5,1)
oxy_5=polyval(P_5,tva)
% [P,S]=polyfit(x,y,1);
% [oxy,DELTA]=polyval(P,tva,S) 
x_2=[x_5(2)  x_5(4)]; %2��궨�µ�Tvaֵ
y_2=[y_5(2)  y_5(4)]; %��������ѹ
P_2=polyfit(x_2,y_2,1)
oxy_2=polyval(P_2,tva)



