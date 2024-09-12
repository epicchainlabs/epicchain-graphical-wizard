# Compiling your Python smart contract with epicchain-boa

To compile python scripts into smart contracts for the Neo blockchain it will be necessary to utilize epicchain-boa.

If you don't have it installed, installing it is quite easy: 
- Download and install Python (if you don't have it yet);
- Install epicchain-boa using pip:
```shell
> pip install epicchain-boa
```

For more information about epicchain-boa check the documentation [here](https://epic-chain.org/docs).

## `src` folder

If you already had epicchain-boa installed, your smart contract should already be compiled. 
Inside `src` you will find the original python script together with the files needed for debugging and deploying to the blockchain: 
- `$_CLASSNAME_$.manifest.json`
- `$_CLASSNAME_$.nef`
- `$_CLASSNAME_$.nefdbgnfo`
- `$_CLASSNAME_$.py`

After changing the smart contract, you'll need to compile it again using epicchain-boa:

```shell
> epicchain-boa <insert path to smart contract>
```

> Note: previously compiled files will be overwritten.

## `test` folder

If you want to test your smart contract before deploying it to MainNet or TestNet, a private chain should already have been created inside the `test` folder. 
Inside it you'll see the following files:
- `$_CONTRACTNAME_$Tests.epicchain-express`
    - The private chain itself, you should find it by its name on the visual tracker.
- `setup-test-chain.batch`
    - This batch was used to transfer some EpicPulse to alice, bob and owner's accounts in the private chain.
