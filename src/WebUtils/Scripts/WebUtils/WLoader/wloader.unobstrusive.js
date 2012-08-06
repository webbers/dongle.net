var WLoader = function ()
{
    this.value = 0;
    this.sources = Array();
    this.sourcesDB = Array();
    this.totalFiles = 0;
    this.loadedFiles = 0;
};

WLoader.prototype.show = function ()
{
    document.getElementById("loadingDiv").style.display = "block";
};

WLoader.prototype.hide = function ()
{
    document.getElementById("loadingZone").style.display = "none";
};

WLoader.prototype.run = function ()
{
    this.show();
    this.loadScript(this.sourcesDB, 0);
};

WLoader.prototype.loadScript = function (source, index)
{
    var currentObj = source[index];
    var currentInstance = this;

    if (currentObj == undefined)
    {
        return;
    }

    var head = document.getElementsByTagName("head")[0];
    var script = document.createElement("script");
    script.type = "text/javascript";
    script.src = currentObj;

    script.onload = function ()
    {
        currentInstance.loaded(currentObj);
        currentInstance.loadScript(source, index + 1);
    };
    script.onreadystatechange = function ()
    {
        if (this.readyState == 'loaded' || this.readyState == 'complete')
        {
            currentInstance.loaded(currentObj);
            currentInstance.loadScript(source, index + 1);
        }
    };
    head.appendChild(script);
};

//Set the value position of the bar (Only 0-100 values are allowed)
WLoader.prototype.setValue = function (value)
{
    if (value >= 0 && value <= 100)
    {
        document.getElementById("progressBar").style.width = value + "%";
        document.getElementById("infoProgress").innerHTML = parseInt(value) + "%";
    }
};

//Add the specified script to the list
WLoader.prototype.addScript = function (source)
{
    this.totalFiles++;
    this.sources[source] = source;
    this.sourcesDB.push(source);
};

//Called when a script is loaded. Increment the progress value and check if all files are loaded
WLoader.prototype.loaded = function (file)
{
    this.loadedFiles++;
    delete this.sources[file];
    var pc = (this.loadedFiles * 100) / this.totalFiles;
    this.setValue(pc);
    //Are all files loaded?
    if (this.loadedFiles == this.totalFiles)
    {
        setTimeout(this.hide, 300);
    }
};

//Unobstrusive calls
window.onload = function ()
{
    var prescripts = document.getElementsByTagName('wscript');
    var wloader = new WLoader();

    for (var i = 0; i < prescripts.length; i++)
    {
        var prescript = prescripts[i];
        wloader.addScript(prescript.attributes['src'].value);
    }
    wloader.run();
};