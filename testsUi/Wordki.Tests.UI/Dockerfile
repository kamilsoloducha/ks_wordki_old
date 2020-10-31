FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

# Disable apt-get warnings
# https://github.com/wv-gis/mudak-wrm-public/issues/2
ENV APT_KEY_DONT_WARN_ON_DANGEROUS_USAGE=DontWarn

# Install certificates so wget/curl request works inside the firewall
#COPY --from=devreg.azurecr.io/certificates / /usr/share/ca-certificates/bg
#WORKDIR /usr/share/ca-certificates
#RUN ls bg/* >> /etc/ca-certificates.conf && update-ca-certificates

# Resolve apt-utils warning
# https://github.com/phusion/baseimage-docker/issues/319#issuecomment-245857919
# https://stackoverflow.com/questions/51023312/docker-having-issues-installing-apt-utils
# ARG DEBIAN_FRONTEND=noninteractive
# RUN apt-get update && apt-get install -yq --no-install-recommends apt-utils

# Install EOL change utility
# RUN apt-get update && apt-get install -yq dos2unix

# Install chrome
RUN wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | apt-key add - &&\
	sh -c 'echo "deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable main" >> /etc/apt/sources.list.d/google.list' &&\
	apt-get update &&\
    apt-get install -yq google-chrome-stable
	
RUN apt-cache policy google-chrome-stable

COPY . .

RUN dotnet restore Wordki.Tests.UI.csproj

ENTRYPOINT dotnet test