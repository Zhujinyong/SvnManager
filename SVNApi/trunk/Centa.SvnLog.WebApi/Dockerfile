FROM microsoft/aspnetcore:2.0
 
RUN cd /usr/local/src
 
RUN mkdir svn
 
WORKDIR /usr/local/src/svn
 
COPY *.* ./
 
EXPOSE 8020
 
CMD ["dotnet", "Centa.SvnLog.WebApi.dll"]

